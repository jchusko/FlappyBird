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

    }
    private void CreatePipe(float height, float xPosition)
    {
        // Create Head
        Transform pipeHead = Instantiate(GameAssets.GetInstance().PFPipeHead);
        pipeHead.position = new Vector3(xPosition, -CAMERA_ORTHO_SIZE + height - PIPE_HEAD_OFFSET);

        // Create Body
        Transform pipeBody = Instantiate(GameAssets.GetInstance().PFPipeBody);
        pipeBody.position = new Vector3(xPosition, -CAMERA_ORTHO_SIZE);
        
        var bodyRenderer = pipeBody.GetComponent<SpriteRenderer>();
        bodyRenderer.size = new Vector2(PIPE_WIDTH, height);

        var pipeBodyBoxCollider = pipeBody.GetComponent<BoxCollider2D>();
        pipeBodyBoxCollider.size = new Vector2(PIPE_WIDTH, height);
        pipeBodyBoxCollider.offset = new Vector2(0f, height * 0.5f);
    }
}
