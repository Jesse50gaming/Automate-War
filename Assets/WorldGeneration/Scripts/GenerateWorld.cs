using UnityEngine;

public class GenerateWorld : MonoBehaviour
{
    public GameObject chunkPrefab;
    public GameObject player;

    public int renderDistance = 16; // 16x16 by default
    public int chunkSize = 16;

    void Start()
    {
        generateWorld();
    }

    void Update()
    {
        for (int x = 0; x < renderDistance; x++)
        {
            for (int z = 0; z < renderDistance; z++)
            {
                GameObject chunkObj = Instantiate(chunkPrefab);

                chunkObj.transform.position = new Vector3(x * chunkSize, 0, z * chunkSize);

                ChunkRenderer chunk = chunkObj.GetComponent<ChunkRenderer>();

                chunk.Init(x * chunkSize, z * chunkSize);
            }
        }
    }

    private void generateWorld()
    {
        for (int x = 0; x < renderDistance; x++)
        {
            for (int z = 0; z < renderDistance; z++)
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