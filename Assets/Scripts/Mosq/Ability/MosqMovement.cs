using UnityEngine;

public class MosqMovement : MonoBehaviour, IMosqAbility
{
    [Header("Scan settings")]
    [SerializeField]
    private int _numDetect;
    [SerializeField]
    private float _scanRadius;
    [SerializeField]
    private float _frequency;
    [SerializeField]
    private ELayers[] _layersToScan;

    [Header("Movement settings")]
    [SerializeField]
    private float _speed;

    // direction variables
    private IMosqDirection[] _dirs;
    private Vector3 _dirFinal;

    // scan variables
    private Collider[] _hits;
    private float _timer;
    private int _layerMask;

    private Mosquito _mosq;

    private void Awake()
    {
        _mosq = GetComponent<Mosquito>();
        _dirs = GetComponents<IMosqDirection>();
        _hits = new Collider[_numDetect];

        foreach (ELayers layerName in _layersToScan)
        {
            int temp = 1 << (int)layerName;
            _layerMask = _layerMask | temp;
        }
    }

    public void ProcessEarlyAbility()
    {
        // timer for all directions 
        _timer += Time.deltaTime;
        if (_timer < _frequency) { return; }
        _timer = 0f;

        // scan surroundings 
        int numHits = Physics.OverlapSphereNonAlloc(transform.position, _scanRadius, _hits, _layerMask);

        // get directions 
        _dirFinal = Vector3.zero;
        foreach (IMosqDirection dir in _dirs)
        {
            _dirFinal += dir.CalcDirection(numHits, _hits);
        }
    }

    public void ProcessAbility()
    {

        if (_mosq.IsInArea())
        {
            // move 
            transform.Translate(_dirFinal.normalized * _speed * Time.deltaTime, Space.Self);
        }
        // if mosq moves out of bounds, reposition it towards the center of the boundary
        else
        {
            Vector3 dir = Vector3.zero;
            // if in room, move towards its rooms center 
            if (transform.parent != null)
            {
                Room room = transform.parent.GetComponent<Room>();
                dir = room.RoomModel.center - transform.localPosition;
            }
            else
            {
                dir = _mosq.AreaCenter - transform.position;
            }

            transform.Translate(dir.normalized * _speed * Time.deltaTime, Space.Self);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, _scanRadius);
    }
}
