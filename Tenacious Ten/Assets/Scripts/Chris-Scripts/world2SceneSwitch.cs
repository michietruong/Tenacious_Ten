﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class world2SceneSwitch : StateMachineBehaviour {
	//OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //Go to name of scene. (You need to change the quotation parameter.)
        Debug.Log("Ive entered world2SceneSwitch");
        string SceneName = "Level_2.0";
        SceneManager.LoadScene(SceneName);
    }
}
