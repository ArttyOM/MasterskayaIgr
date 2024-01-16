using UnityEngine;
using System.Collections;

//This script controls the waves of enemies
public class Emitter : MonoBehaviour
{
    public GameObject[] waves; //Prefabs of the waves of enemies

    private int currentWave; //Which wave we are currently on
    private WaveScript[] waveScripts; //A collection of WaveScripts on the Wave prefabs (this is done for efficiency)


    private void Awake()
    {
        //Create our array
        waveScripts = new WaveScript[waves.Length];
        //Iterate through the wave prefabs
        for (var i = 0; i < waves.Length; i++)
        {
            //Instantiate them
            waves[i] = (GameObject) Instantiate(waves[i], transform.position, Quaternion.identity);
            //Set their parent
            waves[i].transform.parent = transform;
            //Deactivate them
            waves[i].SetActive(false);
            //Save their WaveScript for future use
            waveScripts[i] = waves[i].GetComponent<WaveScript>();
        }
    }

    //This is set up as a coroutine
    private IEnumerator Start()
    {
        //If we have no wave prefabs then exit
        if (waves.Length == 0)
            yield break;

        //Loop indefinitely
        while (true)
        {
            //If the player is currently not playing then wait
            while (!Manager.current.IsPlaying()) yield return new WaitForEndOfFrame();
            //Activate our current wave
            waves[currentWave].SetActive(true);
            //While the wave has active ships wait
            while (waveScripts[currentWave].ShipsStillAlive()) yield return new WaitForEndOfFrame();
            //All enemy ships are inactive so deactivate the wave
            waves[currentWave].SetActive(false);
            //Advance or wrap around the current wave
            if (waves.Length <= ++currentWave)
                currentWave = 0;
        }
    }
}