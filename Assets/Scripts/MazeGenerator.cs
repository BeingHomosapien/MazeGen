using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// [Flags]
[Flags] public enum WallState{
    LEFT = 1,
    RIGHT = 2,
    UP = 4,
    DOWN = 8,
    VISITED = 128,
}

public struct Position{
    public int x;
    public int y;
}

public struct Neighbour{
    public Position Pos;
    public WallState SharedWall;
}

public static class MazeGenerator
{

    private static WallState oppositeSide(WallState wall){
        switch(wall){
            case WallState.LEFT: return WallState.RIGHT;
            case WallState.RIGHT: return WallState.LEFT;
            case WallState.UP: return WallState.DOWN;
            case WallState.DOWN: return WallState.UP;
            default: return WallState.LEFT;
        }
    }

    public static WallState[,] recursiveBacktracker(WallState[,] maze, int width, int height){

        var range = new System.Random(/*Seed*/);
        Stack<Position> positionStack = new Stack<Position>();
        var position = new Position{x = range.Next(0, width), y = range.Next(0, height)};

        positionStack.Push(position);
        maze[position.x, position.y] |= WallState.VISITED;

        while(positionStack.Count > 0){
            var current = positionStack.Pop();
            var neighbours = unvisitedNeighbours(current, maze, width, height);

            if(neighbours.Count > 0){

                positionStack.Push(current);

                var randomInd = range.Next(0, neighbours.Count);
                var randomNeighbour = neighbours[randomInd];

                var neighbourPos = randomNeighbour.Pos;
                maze[current.x, current.y] &= ~randomNeighbour.SharedWall;
                maze[neighbourPos.x, neighbourPos.y] &= ~oppositeSide(randomNeighbour.SharedWall);

                maze[neighbourPos.x, neighbourPos.y] |= WallState.VISITED;
                positionStack.Push(neighbourPos);
            }
        }



        return maze;
    }

    public static List<Neighbour> unvisitedNeighbours(Position p, WallState[,] maze, int width, int height){

        List<Neighbour> neighbours = new List<Neighbour>();

        if(p.x > 0){
            if(!maze[p.x - 1, p.y].HasFlag(WallState.VISITED)){
                neighbours.Add(new Neighbour{
                    Pos = new Position{ x = p.x - 1, y = p.y },
                    SharedWall = WallState.LEFT
                });
            }
        }

        if(p.x < width - 1){
            if(!maze[p.x + 1, p.y].HasFlag(WallState.VISITED)){
                neighbours.Add(new Neighbour{
                    Pos = new Position{ x = p.x + 1, y = p.y },
                    SharedWall = WallState.RIGHT
                });
            }
        }

        if(p.y > 0){
            if(!maze[p.x, p.y - 1].HasFlag(WallState.VISITED)){
                neighbours.Add(new Neighbour{
                    Pos = new Position{ x = p.x, y = p.y - 1 },
                    SharedWall = WallState.DOWN
                });
            }
        }

        if(p.y < height - 1){
            if(!maze[p.x, p.y + 1].HasFlag(WallState.VISITED)){
                neighbours.Add(new Neighbour{
                    Pos = new Position{ x = p.x, y = p.y + 1 },
                    SharedWall = WallState.UP
                });
            }
        }

        return neighbours;

    }

    public static WallState[,] Generate(int width, int height){
        WallState[,] maze = new WallState[width, height];
        WallState initial = WallState.RIGHT | WallState.DOWN | WallState.LEFT | WallState.UP;
        for(int i = 0; i < width; i++){
            for(int j = 0; j < height; j++){
                maze[i, j] =  initial;
            }
        }
        return recursiveBacktracker(maze, width, height);
    }
}
