﻿using PathCreation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Roads
{
    public abstract class Road : MonoBehaviour
    {
        private PathCreator _path;
        public PathCreator Path
        {
            get
            {
                if (_path == null) _path = GetComponent<PathCreator>();
                return _path;
            }
        }

        public float MaxSpeed = 20;
        public Road LeftRoad;
        public Road RightRoad;
        public abstract IEnumerable<RoadTravel> EndTravels { get; }
        public abstract void Exitted(GameObject user);
        public abstract void Entered(GameObject user);

        public abstract IEnumerable<CarMovement> GetUsers();

        public Vector3 this[int i]
        {
            get
            {
                return Path.bezierPath.GetPoint(AnchorToPointIndex(i));
            }
            set
            {
                if (value != this[i])
                {
                    Path.bezierPath.SetPoint(AnchorToPointIndex(i), value, true);
                    var mode = Path.bezierPath.ControlPointMode;
                    Path.bezierPath.ControlPointMode = BezierPath.ControlMode.Free;
                    Path.bezierPath.ControlPointMode = mode;
                    Path.EditorData.PathTransformed();
                    Path.TriggerPathUpdate();
                }
            }
        }

        public abstract bool ContainsUser(GameObject user);

        protected IEnumerable<int> GetSegmentsNumPoints()
        {
            for (int i = 0; i < Path.bezierPath.NumSegments; i++)
            {
                yield return Path.bezierPath.GetPointsInSegment(i).Length;
            }
        }

        public int AnchorToPointIndex(int anchorIndex)
        {
            if (anchorIndex < 0) anchorIndex += Path.bezierPath.NumAnchorPoints;
            int index = 0;
            var arr = GetSegmentsNumPoints().ToArray();
            for (int i = 0; i < anchorIndex; i++)
            {
                index += arr[i] - 1;
            }
            return index;
        }

        public Vector3 GetGlobalPositionPoint(int i)
        {
            return this[i] + transform.position;
        }

        public void SetGlobalPositionPoint(int i, Vector3 value)
        {
            this[i] = value - transform.position;
        }

        public virtual IEnumerable<CarMovement> FindCars(int level)
        {
            foreach (var travel in EndTravels)
            {
                foreach (var car in travel.Road.FindCars(level - 1))
                {
                    yield return car;
                }
            }
        }

        public bool FindUser(GameObject user, int level)
        {
            if (ContainsUser(user)) return true;
            if (level == 1) return false;
            bool result = false;
            Parallel.ForEach(EndTravels, (travel, state) =>
            {
                if (travel.Road.FindUser(user, level - 1))
                {
                    result = true;
                    state.Break();
                }
            });
            //foreach (var travel in EndTravels)
            //{
            //    if (travel.Road.FindUser(user, level - 1)) return true;
            //}
            return result;
        }
    }
}
