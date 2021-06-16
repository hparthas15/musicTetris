using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour
{
            float lastFall = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (!isValidGridPos()) {
            Debug.Log("Game Over");
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update() 
    {

        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            // Modify position
            transform.position += new Vector3(-1, 0, 0);
            // Check Validity
            if (isValidGridPos())
                updateGrid();
            else
                transform.position += new Vector3(1, 0, 0);
        }       
        else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            // Modify position
            transform.position += new Vector3(1, 0, 0);
            // Check Validity
            if (isValidGridPos())
                updateGrid();
            else
                transform.position += new Vector3(-1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            // Modify Rotation
            transform.Rotate(0, 0, -90);
            // Check Validity
            if (isValidGridPos())
                updateGrid();
            else
                transform.Rotate(0, 0, 90);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - lastFall >= 1) {
            // Modify Position
            transform.position += new Vector3(0, -1, 0);
            // Check Validity
            if (isValidGridPos()) {
                updateGrid();
            }
            else {
                Debug.Log("yo");
                // Revert position
                transform.position += new Vector3(0, 1, 0);
                // Delete Filled Horizontal Row
                Playfield.deleteFullRows();
                // Spawn next group
                FindObjectOfType<Spawner>().spawnNext();
                // Disable script
                enabled = false;
            }

            lastFall = Time.time;
        }
    }

    bool isValidGridPos() {
        foreach (Transform child in transform) {
            Vector2 v = Playfield.roundVec2(child.position);

        // Not inside Border?
            if (!Playfield.insideBorder(v))
                return false;
            
            if (Playfield.grid[(int)v.x, (int)v.y] != null && 
                Playfield.grid[(int)v.x, (int)v.y].parent != transform)
                return false;
        }
        return true;
    }
    
    void updateGrid() {
        // Remove old blocks
        for (int y = 0; y < Playfield.h; y++) {
            for (int x = 0; x < Playfield.w; x++) {
                if (Playfield.grid[x, y] != null) {
                    if (Playfield.grid[x, y].parent == transform)
                        Playfield.grid[x, y] = null;
                }
            }
        }

        // Add new blocks
        foreach (Transform child in transform) {
            Vector2 v = Playfield.roundVec2(child.position);
            Playfield.grid[(int)v.x, (int)v.y] = child;
        }
    }

}
