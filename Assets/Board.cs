using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Board{


    public struct Tile{
        public GameObject tileGameObject;
    }
    public int x, y;
    public Tile[][] tile;
    public Board(int x, int y){
        this.x = x;
        this.y = y;
        tile = new Tile[x][];
        for (int i = 0; i < x; i++){
            tile[i] = new Tile[y];
        }
    }
    
}
