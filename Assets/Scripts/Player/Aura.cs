using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aura : MonoBehaviour {
    public float waitBeforePlay = 3f;

    Animator anim;
    Coroutine manager;
    bool loaded;


    // Use this for initialization
    void Start() {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()  {}

    public void StartAura() {
        manager = StartCoroutine(Manager());
        anim.Play("Aura_idle");
    }

    public void StopAura() {
        StopCoroutine(Manager());
        anim.Play("Aura_idle");
        loaded = false;
    }

    public IEnumerator Manager() {
        yield return new WaitForSeconds(waitBeforePlay);
        anim.Play("Aura_Play");
        loaded = true;
    }

    public bool IsLoaded() {
        return loaded;
    }

    
}
