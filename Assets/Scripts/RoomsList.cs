using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomsList : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform _content;
    [SerializeField] private RoomListing _roomListing;

    private List<RoomListing> _listings = new List<RoomListing>();
    private Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo room in cachedRoomList.Values)
        {
            if(room.RemovedFromList)
            {
                int index = _listings.FindIndex(x => x.RoomInfo.Name == room.Name);
                if (index != -1)
                {
                    Destroy(_listings[index].gameObject);   
                    _listings.RemoveAt(index);
                }
            }

            RoomListing roomListing = Instantiate(_roomListing, _content);
            if (roomListing != null)
            {
                roomListing.SetRoomInfo(room);
                _listings.Add(roomListing);
            }
        }
    }
}
