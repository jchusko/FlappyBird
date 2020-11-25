using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using CodeMonkey.Utils;
using Scripts.Enums;

public class Level : MonoBehaviour
{
    private GameState state;
    private const float CAMERA_ORTHO_SIZE = 50;
    private const float PIPE_WIDTH = 7.8f;
    private const float PIPE_HEAD_OFFSET = 1.8f;
    private const float PIPE_MOVEMENT_SPEED = 30f;
    private const float PIPE_DESTROY_X_POSITION = -100f;
    private const float PIPE_SPAWN_X_POSITION = 100f;

    private const float GROUND_SPAWN_X_POSITION = 183f;
    private const float GROUND_DESTROY_X_POSITION = -183f;
    private const float GROUND_SPAWN_Y_POSITION = -49f;
    
    private const float CLOUD_SPAWN_X_POSITION = 100f;
    private const float CLOUD_DESTROY_X_POSITION = -100f;

    private const float BIRD_X_POSITION = 0f;

    private static Level instance;
    public static Level GetInstance() { return instance; }

    
    private List<Transform> groundList;

    private List<Transform> cloudList;
    private float cloudSpawnTimer;
    private float cloudSpawnTimerMax;

    private List<Pipe> pipeList;
    private int pipesSpawned;
    private int pipesPassed;
    private float pipeSpawnTimer;
    private float pipeSpawnTimerMax;
    private float gapSize;

    private void Awake()
    {
        state = GameState.WaitingToStart;

        pipeList = new List<Pipe>();
        groundList = new List<Transform>();
        cloudList = new List<Transform>();
        
        pipeSpawnTimerMax = 1.0f;
        pipesSpawned = 0;
        pipesPassed = 0;
        SetDifficulty(Difficulty.Easy);
        instance = this;
    }

    private void Start()
    {
        Bird.GetInstance().OnGameOver += OnGameOver;
        Bird.GetInstance().OnStartedPlaying += OnStartedPlaying;
        SpawnInitialGround();
        SpawnInitialClouds();
    }
    
    private void OnStartedPlaying(object sender, System.EventArgs e)
    {
        state = GameState.Playing;
    }

    private void OnGameOver(object sender, System.EventArgs e)
    {
        state = GameState.GameOver;        
    }

    private void Update()
    {
        if (state == GameState.Playing)
        {
            HandlePipeMovement();
            HandlePipeSpawning();
            HandleGround();
            HandleClouds();
        }
    }

    private void SpawnInitialClouds()
    {
        cloudList.Add(Instantiate(GameAssets.GetInstance().PFCloud_1, new Vector3(-75f, 20f, 0f), Quaternion.identity));
        cloudList.Add(Instantiate(GameAssets.GetInstance().PFCloud_2, new Vector3(0f, 0f, 0f), Quaternion.identity));
        cloudList.Add(Instantiate(GameAssets.GetInstance().PFCloud_3, new Vector3(75f, 30f, 0f), Quaternion.identity));
    }

    private void HandleClouds()
    {
        foreach(var cloud in cloudList)
        {
            cloud.position += new Vector3(-1, 0, 0) * PIPE_MOVEMENT_SPEED * Time.deltaTime;
            if(cloud.position.x < CLOUD_DESTROY_X_POSITION) 
            { 
                cloud.transform.position = new Vector3(CLOUD_SPAWN_X_POSITION, cloud.position.y, 0); 
            }
        }
    }

    private void SpawnInitialGround()
    {
        groundList.Add(Instantiate(GameAssets.GetInstance().PFGround, new Vector3(0f,    GROUND_SPAWN_Y_POSITION, 0f), Quaternion.identity));
        groundList.Add(Instantiate(GameAssets.GetInstance().PFGround, new Vector3(91.5f, GROUND_SPAWN_Y_POSITION, 0f), Quaternion.identity));
        groundList.Add(Instantiate(GameAssets.GetInstance().PFGround, new Vector3(-91.5f,GROUND_SPAWN_Y_POSITION, 0f), Quaternion.identity));
        groundList.Add(Instantiate(GameAssets.GetInstance().PFGround, new Vector3(183f,  GROUND_SPAWN_Y_POSITION, 0f), Quaternion.identity));
    }
    private void HandleGround()
    {
        foreach(var ground in groundList)
        {
            ground.position += new Vector3(-1, 0, 0) * PIPE_MOVEMENT_SPEED * Time.deltaTime;

            if(ground.position.x < GROUND_DESTROY_X_POSITION)
            {
                ground.transform.position = new Vector3(GROUND_SPAWN_X_POSITION, GROUND_SPAWN_Y_POSITION, 0f);
            }
        }
    }

    private void HandlePipeSpawning()
    {
        pipeSpawnTimer -= Time.deltaTime;
        if(pipeSpawnTimer < 0)
        {
            pipeSpawnTimer += pipeSpawnTimerMax;

            float heightEdgeLimit = 10f;
            float minHeight = gapSize * .5f + heightEdgeLimit;
            float totalHeight = CAMERA_ORTHO_SIZE * 2f;
            float maxHeight = totalHeight - gapSize * .5f - heightEdgeLimit;
            
            float height = Random.Range(minHeight, maxHeight);
            CreateGapPipes(height, gapSize, PIPE_SPAWN_X_POSITION);

            ++pipesSpawned;
            SetDifficulty(GetDifficulty());
        }
    } 

    private void HandlePipeMovement()
    {
        for( int i=0; i<pipeList.Count; ++i)
        {
            
            var pipe = pipeList[i];
            
            bool isToRightOfBird = pipe.GetXPosition() > BIRD_X_POSITION;
            pipe.Move();
            if(isToRightOfBird && pipe.IsBottom() && pipe.GetXPosition() <= BIRD_X_POSITION) 
            { 
                ++pipesPassed;
                SoundManager.PlaySound(Sounds.Score);
            }

            if (pipe.GetXPosition() < PIPE_DESTROY_X_POSITION)
            {
                pipe.Destroy();
                pipeList.RemoveAt(i);
                --i;
            }
        }
    }

    private void SetDifficulty(Difficulty difficulty)
    {
        switch(difficulty)
        {
            case Difficulty.Easy:
                gapSize = 50f;
                pipeSpawnTimerMax = 1.2f;
                break;
            case Difficulty.Medium:
                gapSize = 40f;
                pipeSpawnTimerMax = 1.1f;
                break;
            case Difficulty.Hard:
                gapSize = 33f;
                pipeSpawnTimerMax = 1.0f;
                break;
            case Difficulty.Impossible:
                gapSize = 24f;
                pipeSpawnTimerMax = .8f;
                break;
        }
    }

    private Difficulty GetDifficulty()
    {
        if (pipesSpawned >= 30) { return Difficulty.Impossible; }
        if (pipesSpawned >= 20) { return Difficulty.Hard; }
        if (pipesSpawned >= 10) { return Difficulty.Medium; }
        return Difficulty.Easy;
    } 

    private void CreateGapPipes(float gapY, float gapSize, float xPos)
    {
        CreatePipe(gapY - gapSize * .5f, xPos, true);
        CreatePipe(CAMERA_ORTHO_SIZE * 2f - gapY - gapSize * .5f, xPos, false);
    }

    public int GetPipesPassed()
    {
        return pipesPassed;
    }

    private void CreatePipe(float height, float xPosition, bool createBottom)
    {
        // Create Head
        Transform pipeHead = Instantiate(GameAssets.GetInstance().PFPipeHead);
        float yHeadPosition = (createBottom) ? -CAMERA_ORTHO_SIZE + height - PIPE_HEAD_OFFSET : CAMERA_ORTHO_SIZE - height + PIPE_HEAD_OFFSET;
        pipeHead.position = new Vector3(xPosition, yHeadPosition);

        // Create Body
        Transform pipeBody = Instantiate(GameAssets.GetInstance().PFPipeBody);

        float yBodyPosition;
        if(createBottom)
        {
            yBodyPosition = -CAMERA_ORTHO_SIZE;
        }
        else
        {
            yBodyPosition = CAMERA_ORTHO_SIZE;
            pipeBody.localScale = new Vector3(1, -1, 1);
        }

        pipeBody.position = new Vector3(xPosition, yBodyPosition);
        
        var bodyRenderer = pipeBody.GetComponent<SpriteRenderer>();
        bodyRenderer.size = new Vector2(PIPE_WIDTH, height);

        var pipeBodyBoxCollider = pipeBody.GetComponent<BoxCollider2D>();
        pipeBodyBoxCollider.size = new Vector2(PIPE_WIDTH, height);
        pipeBodyBoxCollider.offset = new Vector2(0f, height * 0.5f);

        pipeList.Add(new Pipe(pipeHead, pipeBody, createBottom));
    }

    private class Pipe
    {
        private Transform pipeHeadTransform;
        private Transform pipeBodyTransform;
        private bool isBottom;

        public Pipe(Transform head, Transform body, bool isBottom)
        {
            pipeHeadTransform = head;
            pipeBodyTransform = body;
            this.isBottom = isBottom;
        }

        public void Move()
        {
            pipeHeadTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVEMENT_SPEED * Time.deltaTime;
            pipeBodyTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVEMENT_SPEED * Time.deltaTime;
        }

        public float GetXPosition()
        {
            return pipeBodyTransform.position.x;
        }

        public void Destroy()
        {
            Object.Destroy(pipeHeadTransform.gameObject);
            Object.Destroy(pipeBodyTransform.gameObject);
        }

        public bool IsBottom()
        {
            return isBottom;
        }
    }
}
