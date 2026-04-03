using UnityEngine;

public class GenerateWorld : MonoBehaviour
{
    public GameObject chunkPrefab;
    public GameObject player;

    public int renderDistance = 16; 
    public int chunkSize = 16;

    private InfiniteGrid<bool> worldGrid;

    void Start()
    {
        worldGrid = new InfiniteGrid<bool>();
        generateWorld(Vector2.zero);
    }

    void Update()
    {
        Vector2 playerChunk = chunkFromBlockPos(
            new Vector2(player.transform.position.x, player.transform.position.z)
        );

        generateWorld(playerChunk);
    }

    private Vector2 chunkFromBlockPos(Vector2 pos)
    {
        return new Vector2(
            Mathf.FloorToInt(pos.x / chunkSize),
            Mathf.FloorToInt(pos.y / chunkSize)
        );
    }

    private void generateWorld(Vector2 chunkCenter)
    {
        int half = renderDistance / 2;

        for (int x = -half; x < half; x++)
        {
            for (int z = -half; z < half; z++)
            {
                int chunkX = (int)chunkCenter.x + x;
                int chunkZ = (int)chunkCenter.y + z;

                if (worldGrid.Get(chunkX, chunkZ)) continue;

                worldGrid.Set(chunkX, chunkZ, true);

                Vector3 worldPos = new Vector3(
                    chunkX * chunkSize,
                    0,
                    chunkZ * chunkSize
                );

                GameObject chunkObj = Instantiate(chunkPrefab, worldPos, Quaternion.identity);

               
                Chunk chunkData = new Chunk(worldPos);

                
                ChunkMesh chunkMesh = chunkObj.GetComponent<ChunkMesh>();
                chunkMesh.Init(chunkData);
            }
        }
    }
}