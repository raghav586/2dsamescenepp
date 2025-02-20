﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInput : MonoBehaviour {

    public Transform Djnode;
    public Transform startNode;
    public Transform endNode;
    public List<Transform> blockPath = new List<Transform>();
    public TrrtComscene trrtPath;
	// Update is called once per frame
	void Update () {
        mouseInput();
    }
    
    /// <summary>
    /// Mouse click.
    /// </summary>
    private void mouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {

            // Update colors for every mouse clicked.
            this.colorBlockPath();
            this.updateNodeColor();

            // Get the raycast from the mouse position from screen.
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.transform.tag == "Node")
            {
                //unmark previous
                Renderer rend;
                if (Djnode != null)
                {
                    rend = Djnode.GetComponent<Renderer>();
                    rend.material.color = Color.white;
                }

                // We now update the selected node.
                Djnode = hit.transform;

                // Mark it
                rend = Djnode.GetComponent<Renderer>();
                rend.material.color = Color.green;

            }
        }
    }

    /// <summary>
    /// Button for Set Starting node.
    /// </summary>
    public void btnStartNode()
    {
        if (Djnode != null)
        {
            DijkstraNode n = Djnode.GetComponent<DijkstraNode>();

            // Making sure only walkable node are able to set as start.
            if (n.isWalkable())
            {
                // If this is a new start node, we will just set it to blue.
                if (startNode == null)
                {
                    Renderer rend = Djnode.GetComponent<Renderer>();
                    rend.material.color = Color.blue;
                }
                else
                {
                    // Reverse the color of the previous node
                    Renderer rend = startNode.GetComponent<Renderer>();
                    rend.material.color = Color.white;

                    // Set the new node as blue.
                    rend = Djnode.GetComponent<Renderer>();
                    rend.material.color = Color.blue;
                }

                startNode = Djnode;
                Djnode = null;
            }
        }
    }

    /// <summary>
    /// Button for Set End node.
    /// </summary>
    public void btnEndNode()
    {
        if (Djnode != null)
        {
            DijkstraNode n = Djnode.GetComponent<DijkstraNode>();

            // Making sure only walkable node are able to set as end.
            if (n.isWalkable())
            {
                // If this is a new end node, we will just set it to cyan.
                if (endNode == null)
                {
                    Renderer rend = Djnode.GetComponent<Renderer>();
                    rend.material.color = Color.cyan;
                }
                else
                {
                    // Reverse the color of the previous node
                    Renderer rend = endNode.GetComponent<Renderer>();
                    rend.material.color = Color.white;

                    // Set the new node as cyan.
                    rend = Djnode.GetComponent<Renderer>();
                    rend.material.color = Color.cyan;
                }

                endNode = Djnode;
                Djnode = null;
            }
        }
    }

   
    // Button for find path.

    public void btnFindPath()
    {   
        // Only find if there are start and end node.
        if (startNode != null && endNode != null)
        {
            // Execute Shortest Path.
           // ShortestPath finder = gameObject.GetComponent<ShortestPath>();
            // List<Transform> paths = finder.findShortestPath(startNode, endNode);

            // Colour the node red.
            trrtPath.BeginSolving(10, startNode, endNode);
            trrtPath.ContinueSolving();

    }
    }

    /// <summary>
    /// Button for blocking a path.
    /// </summary>
    public void btnBlockPath()
    {
        if (Djnode != null)
        {
            // Render the selected node to black.
            Renderer rend = Djnode.GetComponent<Renderer>();
            rend.material.color = Color.black;

            // Set selected node to not walkable
            DijkstraNode n = Djnode.GetComponent<DijkstraNode>();
            n.setWalkable(false);

            // Add the node to the block path list.
            blockPath.Add(Djnode);

            // If the block path is start node, we remove start node.
            if (Djnode == startNode)
            {
                startNode = null;
            }

            // If the block path is end node, we remove end node.
            if (Djnode == endNode)
            {
                endNode = null;
            }

            Djnode = null;
        }

        // For selection grid system.
        UnitSelectionComponent selection = gameObject.GetComponent<UnitSelectionComponent>();
        List<Transform> selected = selection.getSelectedObjects();

        foreach(Transform nd in selected)
        {
            // Render the selected node to black.
            Renderer rend = nd.GetComponent<Renderer>();
            rend.material.color = Color.black;

            // Set selected node to not walkable
            DijkstraNode n = nd.GetComponent<DijkstraNode>();
            n.setWalkable(false);

            // Add the node to the block path list.
            blockPath.Add(nd);

            // If the block path is start node, we remove start node.
            if (nd == startNode)
            {
                startNode = null;
            }

            // If the block path is end node, we remove end node.
            if (nd == endNode)
            {
                endNode = null;
            }
        }

        selection.clearSelections();
    }

    /// <summary>
    /// Button to unblock a path.
    /// </summary>
    public void btnUnblockPath()
    {
        if (Djnode != null)
        {
            // Set selected node to white.
            Renderer rend = Djnode.GetComponent<Renderer>();
            rend.material.color = Color.white;

            // Set selected not to walkable.
            DijkstraNode n = Djnode.GetComponent<DijkstraNode>();
            n.setWalkable(true);

            // Remove selected node from the block path list.
            blockPath.Remove(Djnode);

            Djnode = null;
        }

        // For selection grid system.
        UnitSelectionComponent selection = gameObject.GetComponent<UnitSelectionComponent>();
        List<Transform> selected = selection.getSelectedObjects();

        foreach (Transform nd in selected)
        {
            // Set selected node to white.
            Renderer rend = nd.GetComponent<Renderer>();
            rend.material.color = Color.white;

            // Set selected not to walkable.
            DijkstraNode n = nd.GetComponent<DijkstraNode>();
            n.setWalkable(true);

            // Remove selected node from the block path list.
            blockPath.Remove(nd);
        }

        selection.clearSelections();
    }

    /// <summary>
    /// Clear all the block path.
    /// </summary>
    public void btnClearBlock()
    {   
        // For each blocked path in the list
        foreach(Transform path in blockPath)
        {   
            // Set walkable to true.
            DijkstraNode n = path.GetComponent<DijkstraNode>();
            n.setWalkable(true);

            // Set their color to white.
            Renderer rend = path.GetComponent<Renderer>();
            rend.material.color = Color.white;

        }
        // Clear the block path list and 
        blockPath.Clear();
    }

    /// <summary>
    /// Button to restart level.
    /// </summary>
    public void btnRestart()
    {
        Scene loadedLevel = SceneManager.GetActiveScene();
        SceneManager.LoadScene(loadedLevel.buildIndex);
    }

    /// <summary>
    /// Coloured unwalkable path to black.
    /// </summary>
    private void colorBlockPath()
    {
        foreach(Transform block in blockPath)
        {
            Renderer rend = block.GetComponent<Renderer>();
            rend.material.color = Color.black;
        }
    }

    /// <summary>
    /// Refresh Update Nodes Color.
    /// </summary>
    private void updateNodeColor()
    {
        if (startNode != null)
        {
            Renderer rend = startNode.GetComponent<Renderer>();
            rend.material.color = Color.blue;
        }

        if (endNode != null)
        {
            Renderer rend = endNode.GetComponent<Renderer>();
            rend.material.color = Color.cyan;
        }
    }

}
