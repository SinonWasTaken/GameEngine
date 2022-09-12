namespace NekinuSoft.MeshLoader
{
    public class MeshLoader
    {
        //Loads an .obj mesh files
        public static Mesh loadOBJ(string objFile)
        {
            //Checks if the mesh already exists in memory
            Mesh loadedMesh = Cache.MeshExists(objFile);

            //if it does
            if (loadedMesh != null)
            {
                //The it returns the already existing mesh
                return loadedMesh;
            }

            //If not, then it creates StringReader to read the .obj file 
            StringReader reader = new StringReader(objFile);

            //Creates a variable to read the lines of the file 
            string line = "";
            //List of all verticies, texture coordinates and model normals, as well as the indices to render the model correctly
            List<Vertex> vertices = new List<Vertex>();
            List<Vector2> textures = new List<Vector2>();
            List<Vector3> normals = new List<Vector3>();
            List<int> indices = new List<int>();

            float[] verticesArray = new float[0];
            float[] normalsArray = new float[0];
            float[] texturesArray = new float[0];
            int[] indicesArray = new int[0];

            try
            {
                //While the model file still has a line to read from
                while ((line = reader.ReadLine()) != null)
                {
                    //If the line contains vertices information
                    if (line.StartsWith("v "))
                    {
                        //split the line
                        string[] currentLine = line.Split(" ");
                        //and create a vector3 with the values
                        Vector3 vertex = new Vector3(float.Parse(currentLine[1]),
                            float.Parse(currentLine[2]), float.Parse(currentLine[3]));

                        vertices.Add(new Vertex(vertices.Count, vertex));

                    }
                    //If the line contains texture coordinate information
                    else if (line.StartsWith("vt "))
                    {
                        //split the line
                        string[] currentLine = line.Split(" ");
                        //Creates a vector2 with the information 
                        Vector2 texture = new Vector2(float.Parse(currentLine[1]),
                            float.Parse(currentLine[2]));
                        textures.Add(texture);
                    }
                    //If the line contains normal information
                    else if (line.StartsWith("vn "))
                    {
                        //split the line
                        string[] currentLine = line.Split(" ");
                        //and create a vector3 with the values
                        Vector3 normal = new Vector3(float.Parse(currentLine[1]),
                            float.Parse(currentLine[2]), float.Parse(currentLine[3]));
                        normals.Add(normal);
                    }
                    //If the line contains face information
                    else if (line.StartsWith("f "))
                    {
                        string[] currentLine = line.Split(" ");
                        string[] vertex1 = currentLine[1].Split("/");
                        string[] vertex2 = currentLine[2].Split("/");
                        string[] vertex3 = currentLine[3].Split("/");

                        Vertex v1 = processVertex(vertex1, vertices, indices);
                        Vertex v2 = processVertex(vertex2, vertices, indices);
                        Vertex v3 = processVertex(vertex3, vertices, indices);
                        
                        calculateTangents(v1, v2, v3, textures);
                    }
                }

            }
            catch (Exception e)
            {
                //Generates a crash report when the mesh cant be loaded, either because it does not exist, or the model is incorrectly formatted
                Crash_Report.generate_crash_report($"Couldn't read model file: {objFile}. Error {e}");
                //Exit code -300 means a mesh error
                Environment.Exit(-300);
                return null;
            }

            //Closes the string reader
            reader.Close();

            //I think this removes vertices that arent connected to anything
            removeUnusedVertices(vertices);
            //Creates a new vertices array with the size of vertices * 3 (the number of values in a vector3)
            verticesArray = new float[vertices.Count * 3];
            //Creates a new texture array with the size of vertices * 2 (the number of values in a vector2)
            texturesArray = new float[vertices.Count * 2];
            //Creates a new normal array with the size of vertices * 3 (the number of values in a vector3)
            normalsArray = new float[vertices.Count * 3];
            
            float[] tangentsArray = new float[vertices.Count * 3];
            //The variable isn't used in this case.
            float furthest = convertDataToArrays(vertices, textures, normals, verticesArray, texturesArray, normalsArray, tangentsArray);
            indicesArray = convertIndicesListToArray(indices);
            //Creates the mesh
            Mesh m = Mesh.CreateNewMesh(objFile, indicesArray, verticesArray, texturesArray, normalsArray, tangentsArray);

            return m;
        }
        
        //Calculates the tangent for a point in the mesh 
        private static void calculateTangents(Vertex v0, Vertex v1, Vertex v2, List<Vector2> textures)
        {
            Vector3 delatPos1 = v1.getPosition() - v0.getPosition();
            Vector3 delatPos2 = v2.getPosition() - v0.getPosition();
            Vector2 uv0 = textures[v0.getTextureIndex()];
            Vector2 uv1 = textures[v1.getTextureIndex()];
            Vector2 uv2 = textures[v2.getTextureIndex()];
            Vector2 deltaUv1 = uv1 - uv0;
            Vector2 deltaUv2 = uv2 - uv0;

            float r = 1.0f / (deltaUv1.x * deltaUv2.y - deltaUv1.y * deltaUv2.x);
            delatPos1 *= deltaUv2.y;
            delatPos2 *= deltaUv1.y;
            Vector3 tangent = delatPos1 - delatPos2;
            tangent *= r;
            v0.addTangent(tangent);
            v1.addTangent(tangent);
            v2.addTangent(tangent);
        }

        //Process which vertex will be used with which face
        private static Vertex processVertex(string[] vertex, List<Vertex> vertices, List<int> indices)
        {
            int index = int.Parse(vertex[0]) - 1;
            Vertex currentVertex = vertices[index];
            int textureIndex = int.Parse(vertex[1]) - 1;
            int normalIndex = int.Parse(vertex[2]) - 1;

            if (!currentVertex.isSet())
            {
                currentVertex.setTextureIndex(textureIndex);
                currentVertex.setNormalIndex(normalIndex);
                indices.Add(index);
                return currentVertex;
            }
            else
            {
                return dealWithAlreadyProcessedVertex(currentVertex, textureIndex, normalIndex, indices, vertices);
            }
        }

        //Converts the int list to a int array
        private static int[] convertIndicesListToArray(List<int> indices)
        {
            int[] indicesArray = new int[indices.Count];
            for (int i = 0; i < indicesArray.Length; i++)
            {
                indicesArray[i] = indices[i];
            }

            return indicesArray;
        }

        //Takes the data from the 4 lists, creates above, and moves them to the 4 arrays. The default method
        private static float convertDataToArrays(List<Vertex> vertices, List<Vector2> textures, List<Vector3> normals, float[] verticesArray, float[] texturesArray, float[] normalsArray, float[] tangentsArray) 
        {
            float furthestPoint = 0;
            for (int i = 0; i < vertices.Count; i++) 
            {
                Vertex currentVertex = vertices[i];
                if (currentVertex.getLength() > furthestPoint) 
                {
                    furthestPoint = currentVertex.getLength();
                }
                Vector3 position = currentVertex.getPosition();
                Vector2 textureCoord = textures[currentVertex.getTextureIndex()];
                Vector3 normalVector = normals[currentVertex.getNormalIndex()];
                Vector3 tangent = currentVertex.getAverageTangent();
                verticesArray[i * 3] = position.x;
                verticesArray[i * 3 + 1] = position.y;
                verticesArray[i * 3 + 2] = position.z;
                texturesArray[i * 2] = textureCoord.x;
                texturesArray[i * 2 + 1] = 1 - textureCoord.y;
                normalsArray[i * 3] = normalVector.x;
                normalsArray[i * 3 + 1] = normalVector.y;
                normalsArray[i * 3 + 2] = normalVector.z;
                tangentsArray[i * 3] = tangent.x;
                tangentsArray[i * 3 + 1] = tangent.y;
                tangentsArray[i * 3 + 2] = tangent.z;

            }
            return furthestPoint;
        }

        //Used with models that dont contain normal values. UI models specifically
        private static float convertDataToArrays(List<Vertex> vertices, List<Vector2> textures,
            float[] verticesArray, float[] texturesArray)
        {
            float furthestPoint = 0;
            for (int i = 0; i < vertices.Count; i++)
            {
                Vertex currentVertex = vertices[i];

                if (currentVertex.getLength() > furthestPoint)
                {
                    furthestPoint = currentVertex.getLength();
                }

                Vector3 position = currentVertex.getPosition();
                Vector2 textureCoord = textures[currentVertex.getTextureIndex()];
                verticesArray[i * 3] = position.x;
                verticesArray[i * 3 + 1] = position.y;
                verticesArray[i * 3 + 2] = position.z;
                texturesArray[i * 2] = textureCoord.x;
                texturesArray[i * 2 + 1] = 1 - textureCoord.y;

            }

            return furthestPoint;
        }

        //Removes duplicate vertices. Since only models that are entirely composed of triangles, two triangles will contain the same point used in a face
        private static Vertex dealWithAlreadyProcessedVertex(Vertex previousVertex, int newTextureIndex,
            int newNormalIndex,
            List<int> indices, List<Vertex> vertices)
        {
            if (previousVertex.hasSameTextureAndNormal(newTextureIndex, newNormalIndex))
            {
                indices.Add(previousVertex.getIndex());
                return previousVertex;
            }
            else
            {
                Vertex anotherVertex = previousVertex.getDuplicateVertex();
                if (anotherVertex != null)
                {
                    return dealWithAlreadyProcessedVertex(anotherVertex, newTextureIndex, newNormalIndex, indices,
                        vertices);
                }
                else
                {
                    Vertex duplicateVertex = new Vertex(vertices.Count, previousVertex.getPosition());
                    duplicateVertex.setTextureIndex(newTextureIndex);
                    duplicateVertex.setNormalIndex(newNormalIndex);
                    previousVertex.setDuplicateVertex(duplicateVertex);
                    vertices.Add(duplicateVertex);
                    indices.Add(duplicateVertex.getIndex());
                    return duplicateVertex;
                }
            }
        }

        private static void removeUnusedVertices(List<Vertex> vertices)
        {
            foreach (Vertex vertex in vertices)
            {
                vertex.averageTangents();
                if (!vertex.isSet())
                {
                    vertex.setTextureIndex(0);
                    vertex.setNormalIndex(0);
                }
            }
        }
    }
}