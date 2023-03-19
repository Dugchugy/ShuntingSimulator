using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public GameObject[] menuLayers;

    void Start(){
        //moves to main menu on scene start
        gotoMain();
    }

    public void gotoLevelSelect(){
        //changes to layer 1 (level select)
        changeLayer(1);
    }

    public void selectLevel(int level){
        
    }

    public void gotoMain(){
        //changes to layer 0 (main)
        changeLayer(0);
    }

    private void changeLayer(int layer){
        //loops through each menu layer
        for(int i = 0; i < menuLayers.Length; i++){
            //checks if the current menu layer is the one to enable
            if(i == layer){
                //activates the desired menu layer
                menuLayers[i].SetActive(true);
            }else{
                //deactivates the other menu layers
                menuLayers[i].SetActive(false);

            }

        }
    }
}
