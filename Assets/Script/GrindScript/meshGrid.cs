
using UnityEngine;

public class meshGrid : MonoBehaviour
{

    public enum FunctionOption
    {
        Linear,
        Exponential,
        Parabola,
        Sphere,
        Sine,
        Ripple
    }

    private delegate float FunctionDelegate(Vector3 p, float t);
    private static FunctionDelegate[] functionDelegates = {
        Linear,
        Exponential,
        Parabola,
        Sphere,
        Sine,
        Ripple
    };

    private static bool negShape = false;
    public FunctionOption function;

    [Range(10, 100)]
    public int resolution = 10;

    private int currentResolution;
    private ParticleSystem.Particle[] points;

    private void CreatePoints()
    {
        currentResolution = resolution;
        points = new ParticleSystem.Particle[resolution * resolution];
        //  float increment = 1f / (resolution - 1);
       // float[] increment = { (-1f / (resolution - 1)), (1f / (resolution - 1)) };
//        float negIncrement = 1f / (resolution - 1);
        int i = 0;
        int j = 0;
        float lowest= -2f;
        float[] hold = new float[resolution];
        while (hold[resolution-1] <= 2)
        {
            lowest += 1f / (resolution - 1);
            hold[j] = lowest;
            
        }
        for (int x = 0; x < resolution; x++)
        {
            for (int z = 0; z < resolution; z++)
            {
               // print(x*increment[1]);
                //print(x * increment[0]);
              //  Vector3 n = new Vector3(x * increment[0], 0f, z * increment[0]);
                Vector3 p = new Vector3(x * hold[i], 0f, z * hold[i]);
                //Vector3 hold;
                points[i].position = p;
                points[i].color = new Color(p.x, 0f, p.z);
                points[i++].size = 0.1f;
            }
        }
    }

    void Update()
    {
        if (currentResolution != resolution || points == null)
        {
            CreatePoints();
        }
        FunctionDelegate f = functionDelegates[(int)function];
        float t = Time.timeSinceLevelLoad;
        for (int i = 0; i < points.Length; i++)
        {
            Vector3 p = points[i].position;
            p.y = f(p, t);
            points[i].position = p;
            Color c = points[i].color;
            c.g = p.y;
            points[i].color = c;
        }
        GetComponent<ParticleSystem>().SetParticles(points, points.Length);
    }

    private static float Linear(Vector3 p, float t)
    {
        return p.x;
    }

    private static float Exponential(Vector3 p, float t)
    {
        return p.x * p.x;
    }
    private static float Sphere(Vector3 p, float t)
    {
        //x^2+y^2+z^2 = 1
        // y = sqrt(1-x^2-z^2)
        // return Mathf.Sqrt(1f - Mathf.Pow(p.x, 2) - Mathf.Pow(p.z, 2));
        negShape = true;
        return Mathf.Sqrt(1f-p.x*p.x-p.z*p.z);
    }

    private static float Parabola(Vector3 p, float t)
    {
        p.x = 2f * p.x - 1f;
        p.z = 2f * p.z - 1f;
        return 1f - p.x * p.x * p.z * p.z;
    }

    private static float Sine(Vector3 p, float t)
    {
        return 0.50f +
            0.25f * Mathf.Sin(4 * Mathf.PI * p.x + 4 * t) * Mathf.Sin(2 * Mathf.PI * p.z + t) +
            0.10f * Mathf.Cos(3 * Mathf.PI * p.x + 5 * t) * Mathf.Cos(5 * Mathf.PI * p.z + 3 * t) +
            0.15f * Mathf.Sin(Mathf.PI * p.x + 0.6f * t);
    }

    private static float Ripple(Vector3 p, float t)
    {
        p.x -= 0.5f;
        p.z -= 0.5f;
        float squareRadius = p.x * p.x + p.z * p.z;
        return 0.5f + Mathf.Sin(15f * Mathf.PI * squareRadius - 2f * t) / (2f + 100f * squareRadius);
    }
}

//////////////////////////////////// GetComponent<ParticleSystem>().SetParticles(points, points.Length);///////////////////////////////////////