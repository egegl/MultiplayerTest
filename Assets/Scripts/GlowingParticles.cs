using System.Collections.Generic;
using UnityEngine;

/* 
THIS SCRIPT IS WRITTEN BY A UNITY EMPLOYEE BCS THERE IS NO OFFICIAL WAY TO MAKE A PARTICLE SYSTEM GLOW 
(IN 2D AT LEAST) SO I JUST COPIED IT FROM THE FORUM AND CHANGED IT A BIT
*/

[RequireComponent(typeof(ParticleSystem))]
public class GlowingParticles : MonoBehaviour
{
    public GameObject _Prefab;

    private ParticleSystem _ParticleSystem;
    private List<GameObject> _Instances = new List<GameObject>();
    private ParticleSystem.Particle[] _Particles;

    // Start is called before the first frame update
    void Start()
    {
        _ParticleSystem = GetComponent<ParticleSystem>();
        _Particles = new ParticleSystem.Particle[_ParticleSystem.main.maxParticles];
    }

    // Update is called once per frame
    void LateUpdate()
    {
        int count = _ParticleSystem.GetParticles(_Particles);

        while (_Instances.Count < count)
            _Instances.Add(Instantiate(_Prefab, _ParticleSystem.transform));

        bool worldSpace = (_ParticleSystem.main.simulationSpace == ParticleSystemSimulationSpace.World);
        for (int i = 0; i < _Instances.Count; i++)
        {
            if (i < count)
            {
                if (worldSpace)
                    _Instances[i].transform.position = _Particles[i].position;
                else
                    _Instances[i].transform.localPosition = _Particles[i].position;
                _Instances[i].SetActive(true);
            }
            else
            {
                _Instances[i].SetActive(false);
            }
        }
    }
}