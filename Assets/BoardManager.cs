using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField]
    GameObject green, red, blue, yellow, orange, white;
    GameObject[] prefabs;
    public Camera mainCamera;
    public static Board board;
    [SerializeField]
    public static int x = 6, y = 7;
    public int scrambleTimes;
    [SerializeField]
    private int xSerialized = 6, ySerialized = 6;                  //don't use for anything else
    
    void Awake(){

    }

    void Start(){
        x = xSerialized;
        y = ySerialized;
        board = new Board(x, y);
        float yCam = (y - 1)/2;
        mainCamera = Camera.main;
        mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, yCam, mainCamera.transform.position.z);
        prefabs = new GameObject[6]{green, red, blue, yellow, orange, white};        
        int i = 0, j = 0;
        foreach (Board.Tile[] coordX in board.tile){
            foreach(Board.Tile coord in coordX){ 
                GameObject go = GameObject.Instantiate(prefabs[i%6], new Vector3(i, j, 0), Quaternion.identity);
                board.tile[i][j].tileGameObject = go;
                //int number = Random.Range(0,6);
                //if (GameO)
                //Board.coord[x][y].objeto = ;
                j++;
            }
            i++;
            j = 0;
        }
        StartCoroutine(Scramble(scrambleTimes));
    }

    public static IEnumerator Scramble(int times){
        bool[] isHorizontalArray = new bool[2]{true, false};
        int[] xIndexArray = new int[x];
        int[] yIndexArray = new int[y];
        int[] signArray = new int[2]{-1, 1};
        bool isHorizontal;
        for (int i = 0; i < x; i++){
            xIndexArray[i] = i;
        }
        for (int i = 0; i < y; i++){
            yIndexArray[i] = i;
        }
        for (int i = 0; i < times; i++){
            isHorizontal = isHorizontalArray[Random.Range(0, 2)];
            if (!isHorizontal){
                MoveObjects(isHorizontal, xIndexArray[Random.Range(0, x)], signArray[Random.Range(0,1)]);
            } else {
                MoveObjects(isHorizontal, yIndexArray[Random.Range(0, y)], signArray[Random.Range(0,1)]);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public static void MoveObjects(bool isHorizontal, int index, int sign){
        if (!isHorizontal && index % 2 == 0){
            int lenght = board.tile[index].Length;
            Board.Tile[] aux = new Board.Tile[lenght];
            for (int k = 0; k < lenght; k++){
                aux[k] = board.tile[index][k];  
            }            for (int i = 0; i < lenght; i++){
                Board.Tile position = board.tile[index][i];
                Vector3 pos = aux[i].tileGameObject.transform.position;
                if (i - sign >= 0 && i - sign < lenght){ //verifica se é de alguma das pontas
                    aux[i].tileGameObject.transform.position = new Vector3(pos.x, pos.y - (sign), pos.z);
                    board.tile[index][i - (sign)].tileGameObject = aux[i].tileGameObject; 
                } else {
                    if (sign > 0){
                        aux[i].tileGameObject.transform.position = new Vector3(pos.x, lenght - 1, pos.z);
                        board.tile[index][lenght - 1].tileGameObject =  aux[i].tileGameObject;
                    }
                    else {
                        aux[i].tileGameObject.transform.position = new Vector3(pos.x, 0, pos.z);
                        board.tile[index][0].tileGameObject =  aux[i].tileGameObject;
                    }
                }
            }
        }
        else if (isHorizontal && index % 2 == 0) {
            int lenght = board.tile.Length;
            Board.Tile[] aux = new Board.Tile[lenght];
            for (int k = 0; k < lenght; k++){
                aux[k] = board.tile[k][index];  
            }
            for (int i = 0; i < lenght; i++){
                Board.Tile position = board.tile[i][index];
                Vector3 pos = aux[i].tileGameObject.transform.position;
                if (i - sign >= 0 && i - sign < lenght){ //verifica se é de alguma das pontas
                    aux[i].tileGameObject.transform.position = new Vector3(pos.x - (sign), pos.y, pos.z);
                    board.tile[i - (sign)][index].tileGameObject = aux[i].tileGameObject; 
                } else {
                    if (sign > 0){
                        aux[i].tileGameObject.transform.position = new Vector3(lenght - 1, pos.y, pos.z);
                        board.tile[lenght - 1][index].tileGameObject =  aux[i].tileGameObject;
                    }
                    else {
                        aux[i].tileGameObject.transform.position = new Vector3(0, pos.y, pos.z);
                        board.tile[0][index].tileGameObject =  aux[i].tileGameObject;
                    }
                }
            }
        }
    }
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        
    }
}
