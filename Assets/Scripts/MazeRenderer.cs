using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeRenderer : MonoBehaviour
{
    
    [SerializeField]
    [Range(1, 50)]
    private int width = 10;

    [SerializeField]
    [Range(1, 50)]
    private int height = 10;

    [SerializeField]
    private float size = 1f;

    [SerializeField]
    private Transform wall = null;
    [SerializeField]
    private Transform floorPrefab = null;

    void Start()
    {
        var floor = Instantiate(floorPrefab, transform);
        floor.localScale = new Vector3(width, 1, height);   
        var maze = MazeGenerator.Generate(width, height);
        Draw(maze);
    }

    private void Draw(WallState[,] maze){

        for(int i = 0; i < width; i++){
            for(int j = 0; j < height; j++){
                var cell = maze[i, j];
                var position = new Vector3(-width/2 + i, 0, -height/2 + j);

                if(cell.HasFlag(WallState.UP)){
                    var topWall = Instantiate(wall, transform) as Transform;
                    topWall.position = position + new Vector3(0, 0, size/2);
                    topWall.localScale = new Vector3(size, topWall.localScale.y, topWall.localScale.z);
                }

                if(cell.HasFlag(WallState.LEFT)){
                    var leftWall = Instantiate(wall, transform) as Transform;
                    leftWall.position = position + new Vector3(-size/2, 0, 0);
                    leftWall.eulerAngles = new Vector3(0, 90, 0);
                }

                if(i == width - 1 && cell.HasFlag(WallState.RIGHT) ){
                    var rightWall = Instantiate(wall, transform) as Transform;
                    rightWall.position = position + new Vector3(size/2, 0, 0);
                    rightWall.eulerAngles = new Vector3(0, 90, 0);
                }

                if(j == 0 && cell.HasFlag(WallState.DOWN)){
                    var bottomWall = Instantiate(wall, transform) as Transform;
                    bottomWall.position = position + new Vector3(0, 0, -size/2);
                    bottomWall.localScale = new Vector3(size, bottomWall.localScale.y, bottomWall.localScale.z);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
