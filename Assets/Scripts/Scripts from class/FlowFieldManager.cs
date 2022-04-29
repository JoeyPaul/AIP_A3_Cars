using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowFieldManager : MonoBehaviour
{
    List<List<float>> ffA = new List<List<float>>(); // index
    List<List<float>> ffM = new List<List<float>>(); // index
    public int width = 10;
    public int height = 10;

    void Start()
    {
        float previousA = Random.Range(0, 360);
        float previousM = Random.Range(0.2f, 2.5f);

        for (int y = 0; y < height; ++y)
        {
            List<float> rowA = new List<float>();
            List<float> rowM = new List<float>();

            for (int x = 0; x < width; ++x)
            {
                float current = previousA + Random.Range(-12, 12);
                rowA.Add(current);

                float currentM = Mathf.Clamp(previousM + Random.Range(-0.1f, 0.1f), 0.1f, 2.5f);
                rowM.Add(currentM);

                previousA = current;
            }

            ffA.Add(rowA);
            ffM.Add(rowM);
        }
    }
    void Update()
    {
        for (int y = 0; y < height; ++y)
        {
            List<float> row = ffA[y];

            for (int x = 0; x < width; ++x)
            {
                float y_pos = y + 0.5f;
                float x_pos = x + 0.5f;

                Vector3 force = GetForceForPosition(new Vector3(x, y, 0.0f));
                Debug.DrawRay(new Vector3(x_pos, y_pos, 0.0f), force, Color.cyan);

                /*

                float force_angle = row[x];

                Vector3 force_direction = new Vector3(Mathf.Cos(force_angle * Mathf.Deg2Rad), Mathf.Sin(force_angle * Mathf.Deg2Rad), 0.0f);
                force_direction *= 0.5f;

                Debug.DrawRay(new Vector3(x_pos, y_pos, 0.0f), force_direction, Color.cyan);
                */
            }
        }
    }

    public Vector3 GetForceForPosition(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x); // 0
        int y = Mathf.FloorToInt(position.y);

        List<float> rowA = ffA[y];
        float force_angle = rowA[x];
        List<float> rowM = ffM[y];
        float force_magnitude = rowM[x];

        Vector3 force_direction = new Vector3(Mathf.Cos(force_angle * Mathf.Deg2Rad), Mathf.Sin(force_angle * Mathf.Deg2Rad), 0.0f);

        return force_direction * force_magnitude;
    }
}
