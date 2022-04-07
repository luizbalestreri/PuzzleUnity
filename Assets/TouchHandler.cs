using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchHandler : MonoBehaviour
{
    bool isHorizontal = false;
    public int arr1, arr2;
    int index = 0;
    [SerializeField]
    bool swipeCoroutineIsRuning = false;
    InputController inputManager;
    Camera cameraMain;
    Vector2 startPosition;
    float startTime;
    private Coroutine Swipe;
    public int firstSerial;

    void Update(){
        firstSerial = LineManager.first[0];
    }
    void Awake(){
        inputManager = InputController.Instance;
        cameraMain = Camera.main;
    }

    void OnEnable(){
        inputManager.OnStartTouch += SwipeStart;
        inputManager.OnEndTouch += SwipeEnd;
    }
    void OnDisable(){
        inputManager.OnStartTouch -= SwipeStart;
        inputManager.OnEndTouch -= SwipeEnd;
    }
    private void SwipeStart(Vector2 position, float time){
        startPosition = position;
        startTime = time;
        if (!swipeCoroutineIsRuning){
            Debug.Log(Utils.RoundVector2(inputManager.PrimaryPosition()));
            swipeCoroutineIsRuning = true;
            StartCoroutine("SwipeDirection");
        }
    }

    private void SwipeEnd(Vector2 position, float time){
        Debug.Log("Swipe End");
        StopCoroutine("SwipeCoroutine");
        swipeCoroutineIsRuning = false;
        LineManager.DestroyBeforeAfter();
        Board.Tile[] aux = LineManager.CreateAux(isHorizontal, arr1);
        LineManager.Rearrange(LineManager.first[0], arr1, aux, isHorizontal);
        aux = LineManager.CreateAux(isHorizontal, arr2);
        LineManager.Rearrange(LineManager.first[1], arr2, aux, isHorizontal);
        LineManager.first[0] = LineManager.first[1] = 0;
    }
    private IEnumerator SwipeDirection(){
        while(true){
            if (System.Math.Abs(startPosition.x - inputManager.PrimaryPosition().x) > 0.2f || System.Math.Abs(startPosition.y - inputManager.PrimaryPosition().y) > 0.2f){
                if (System.Math.Abs(startPosition.x - inputManager.PrimaryPosition().x) > System.Math.Abs(startPosition.y - inputManager.PrimaryPosition().y)){
                    isHorizontal = true;
                    index = (int) System.Math.Round(inputManager.PrimaryPosition().y);
                }
                else {
                    isHorizontal = false;
                    index = (int) System.Math.Round(inputManager.PrimaryPosition().x);
                }
                StartCoroutine("SwipeCoroutine");
            }
            yield return null;    
        }
    }
    private IEnumerator SwipeCoroutine(){
        //onde parei, precisa setar quais sao as duas linhas que serao mexidas, before e after.
        int lenght = isHorizontal ? BoardManager2.x : BoardManager2.y;
        if (BoardManager2.MoveConstraints(isHorizontal, index) == true){
            LineManager.last[0] = LineManager.last[1] =  lenght - 1; 
            LineManager.first[0] = LineManager.first[1] = 0;
            if (LineManager.before[0] != null) GameObject.Destroy(LineManager.before[0]);
            if (LineManager.before[1] != null) GameObject.Destroy(LineManager.before[1]);
            if (LineManager.after[0] != null) GameObject.Destroy(LineManager.after[0]);
            if (LineManager.after[1] != null) GameObject.Destroy(LineManager.after[1]);
            
            if (index == 0 || index == lenght - 1){arr1 = 0; arr2 = lenght - 1;} //verifica se é alguma das pontas, para movimentar as duas pontas.
            else {
                if ((index - 2) % 3 != 0) index -= 1;
                arr1 = index; arr2 = index + 1;  //verifica se é o primeiro ou o segundo entre os dois que vais      
            }
            while(true){
                float dif = isHorizontal? (inputManager.PrimaryPosition().x - startPosition.x) :
                                        (inputManager.PrimaryPosition().y - startPosition.y);
                for (float i = 0; i < System.Math.Abs(dif); i+=0.5f){
                    LineManager.MoveObjects(0, isHorizontal, arr1, dif);
                    LineManager.MoveObjects(1, isHorizontal, arr2, dif);
                }
                startPosition = inputManager.PrimaryPosition();
                yield return null;
            }
        }
    }
}


// - aparecer em um lado 
// - aparecer no lado oposto
// - trocar o tile reserva quando completa a volta
// - trocar o tile no tabuleiro quando completa a volta
// OBS: se deixar pra trocar os tiles depois que acabar o movimento, não tem como controlar utilizando tile[lenght].pos + 1 pro teleport.