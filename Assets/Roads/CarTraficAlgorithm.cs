﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Roads
{
    [ExecuteInEditMode]
    public class CarTraficAlgorithm : MonoBehaviour
    {
        public List<ParallelSafeCarMove> CarMovements;
        // Start is called before the first frame update
        void Start()
        {
            CarMovements = new List<ParallelSafeCarMove>();
            foreach (var car in GameObject.FindGameObjectsWithTag("Car"))
            {
                if (car.TryGetComponent(out ParallelSafeCarMove movement))
                {
                    CarMovements.Add(movement);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            Parallel.ForEach(CarMovements, (movements) =>
            {
                movements.UpdateSpeed();
                movements.BasicUpdate();
            });
        }
    }
}