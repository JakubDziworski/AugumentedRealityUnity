using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Object = UnityEngine.Object;

namespace Assets.Utils
{
    class TrackedObjectsManager
    {
        private readonly IDictionary<ARMarker, ICollection<ARTrackedObject>> objectsForMarkers;

        private TrackedObjectsManager()
        {
            objectsForMarkers = new Dictionary<ARMarker, ICollection<ARTrackedObject>>();
        }

        #region SingletonBoilerPlate

        private static TrackedObjectsManager mInstance;

        public static TrackedObjectsManager Instance
        {
            get
            {
                if(mInstance == null) mInstance = new TrackedObjectsManager();
                return mInstance;
            }
        }

        #endregion SingletonBoilerPlate

        public void AddTrackedObject(ARTrackedObject trackedObject)
        {
            ARMarker marker = trackedObject.GetMarker();
            if (objectsForMarkers.ContainsKey(marker))
            {
                objectsForMarkers[marker].Add(trackedObject);
            }
            else
            {
                objectsForMarkers.Add(marker, new HashSet<ARTrackedObject> {trackedObject});
            }
        }

        public void RemoveTrackedObject(ARTrackedObject trackedObject)
        {
            ARMarker marker = trackedObject.GetMarker();
            objectsForMarkers[marker].Remove(trackedObject);
            if (objectsForMarkers[marker].Count == 0)
            {
                objectsForMarkers.Remove(marker);
            }
        }

        public IEnumerable<ARTrackedObject> GetTrackedObjectsByMarker(ARMarker marker)
        {
            return objectsForMarkers[marker];
        }
    }
}
