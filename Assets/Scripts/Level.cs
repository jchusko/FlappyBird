using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    private const float CAMERA_ORTHO_SIZE = 50;
    private const float PIPE_WIDTH = 7.8f;
    private const float PIPE_HEAD_OFFSET = 1.8f;
    private const float PIPE_MOVEMENT_SPEED = 30f;
    private const float PIPE_DESTROY_X_POSITION = -100f;
    private const float PIPE_SPAWN_X_POSITION = 100f;
    private const float BIRD_X_POSITION = 0f;

    private static Level instance;
    public static Level GetInstance() { return instance; }

    private List<Pipe> pipeList;
    private int pipesSpawned;
    private int pipesPassed;
    private float pipeSpawnTimer;
    private float pipeSpawnTimerMax;
    private float gapSize;

    public enum Difficulty
    {
        Easy,
        Medium,
        Hard,
        Impossible
    }

    private void Awake()
    {
        pipeList = new List<Pipe>();
        pipeSpawnTimerMax = 1.0f;
        pipesSpawned = 0;
        pipesPassed = 0;
        SetDifficulty(Difficulty.Easy);
        instance = this;
    }

    private void Start()
    {

    }

    private void Update()
    {
        PipeMovement();
        HandlePipeSpawning();
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

    private void PipeMovement()
    {
        for( int i=0; i<pipeList.Count; ++i)
        {
            
            var pipe = pipeList[i];
            
            bool isToRightOfBird = pipe.GetXPosition() > BIRD_X_POSITION;
            pipe.Move();
            if(isToRightOfBird && pipe.IsBottom() && pipe.GetXPosition() <= BIRD_X_POSITION) { ++pipesPassed; }

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
