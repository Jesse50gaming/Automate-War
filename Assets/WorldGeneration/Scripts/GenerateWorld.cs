using UnityEngine;

public class GenerateWorld : MonoBehaviour
{
    public GameObject chunkPrefab;

    public int worldSize = 5; // 5x5 chunks
    public int chunkSize = 16;

    void Start()
    {
        generateWorld();
    }

    private void generateWorld()
    {
        for (int x = 0; x < worldSize; x++)
        {
            for (int z = 0; z < worldSize; z++)
            {
                GameObject chunkObj = Instantiate(chunkPrefab);

                chunkObj.transform.position = new Vector3(
                    x * chunkSize,
                    0,
                    z * chunkSize
                );

                ChunkRenderer chunk = chunkObj.GetComponent<ChunkRenderer>();

                chunk.Init(
                    x * chunkSize,
                    z * chunkSize
                );
            }
        }
    }
}