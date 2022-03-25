using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager2 : MonoBehaviour
{
    [SerializeField]
    GameObject green, red, blue, yellow, orange, white, xGameObject;
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
        
        int p = 0;
        for (int i = 1; i < x; i+= 3){
            GameObject.Instantiate(xGameObject, new Vector3(i, -1, 0), Quaternion.identity);
            GameObject.Instantiate(xGameObject, new Vector3(i, y, 0), Quaternion.identity);
            for (int j = 1; j < y; j+=3){
                for (int k = -1; k < 2; k++){
                    for (int l = -1; l < 2; l++){
                        GameObject gi = GameObject.Instantiate(prefabs[p], new Vector3(i + l, j + k, 0), Quaternion.identity);
                        //if (l == 0 && k == 0) 
                        gi.transform.localScale = new Vector3(1.1f, 1.1f, 1);
                        board.tile[i + l][j + k].tileGameObject = gi;
                    }        
                }
                p++;
            }
        }
        for (int i = 1; i < y; i+=3){
            GameObject.Instantiate(xGameObject, new Vector3(-1, i, 0), Quaternion.identity);
            GameObject.Instantiate(xGameObject, new Vector3(x, i, 0), Quaternion.identity);

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
                MoveConstraints(isHorizontal, xIndexArray[Random.Range(0, x)], signArray[Random.Range(0,1)]);
            } else {
                MoveConstraints(isHorizontal, yIndexArray[Random.Range(0, y)], signArray[Random.Range(0,1)]);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public static void MoveConstraints(bool isHorizontal, int index, float sign){
        Debug.Log(sign);
        int lenght = isHorizontal? lenght = board.tile[0].Length : lenght = board.tile.Length;
        if (index >= 0 && index < lenght){
            if ((index - 1) % 3 != 0){
                if (index == 0 || index == lenght - 1){
                    MoveObjectsFacade(isHorizontal, 0, sign);
                    MoveObjectsFacade(isHorizontal, lenght - 1, sign);
                }
                else {
                    if ((index - 2) % 3 != 0) index -= 1;  
                    MoveObjectsFacade(isHorizontal, index, sign);
                    MoveObjectsFacade(isHorizontal, index + 1, sign);
                }
            }
        }
    }
    private static void MoveObjectsFacade(bool isHorizontal, int index, float sign){
        int lenght;
        Board.Tile[] aux;
        if (!isHorizontal){
            lenght = board.tile[index].Length;
            aux = new Board.Tile[lenght];
        }
        else {
            lenght = board.tile.Length;
            aux = new Board.Tile[lenght];
        }

        for (int k = 0; k < lenght; k++){
            aux[k] = board.tile[isHorizontal?k: index][isHorizontal? index: k];  
        }       
        
        for (int i = 0; i < lenght; i++){
            Movement(i, sign, lenght, isHorizontal, index, aux[i].tileGameObject);
        }
    }
    
     private static void Movement(int i, float sign, int lenght, bool isHorizontal, int index, GameObject aux){
        Board.Tile position = board.tile[isHorizontal?i: index][isHorizontal? index: i];
        Vector3 pos = aux.transform.position;
        if (i - sign >= 0 && i - sign < lenght){ //verifica se é de alguma das pontas
            aux.transform.position = isHorizontal? new Vector3(pos.x - (sign), pos.y, pos.z) : new Vector3(pos.x, pos.y - (sign), pos.z);
            board.tile[isHorizontal? (int) (i - (sign)): index][isHorizontal? index: (int)(i - (sign))].tileGameObject = aux; 
        } else {
            if (sign > 0){
                aux.transform.position = isHorizontal? new Vector3(lenght - 1, pos.y, pos.z) : new Vector3(pos.x, lenght - 1, pos.z);
                board.tile[isHorizontal? lenght - 1: index][isHorizontal? index: lenght - 1].tileGameObject =  aux;
            }
            else {
                aux.transform.position = isHorizontal? new Vector3(0, pos.y, pos.z) : new Vector3(pos.x, 0, pos.z);
                board.tile[isHorizontal? 0: index][isHorizontal? index: 0].tileGameObject =  aux;
            }
        }
    }
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        
    }
}
