using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    private const float CAMERA_ORTHO_SIZE = 50;
    private const float PIPE_WIDTH = 7.8f;
    private const float PIPE_HEAD_OFFSET = 1.8f;
    private void Start()
    {
        CreatePipe(40f, 20, true);
        CreatePipe(40f, 20, false);
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
    }
}
