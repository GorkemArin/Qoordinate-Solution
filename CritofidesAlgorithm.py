from QoordinateClasses import *

class Node:

    def __init__(self, index: int, name: str=''):
        self.name = name
        self.index = index
        self.children = []
        self.child_dist = []

    def Add(self, node, distance: float):
        self.children.append(node)
        self.child_dist.append(distance)

    def GetChildren(self):
        return self.children

    def GetName(self) -> str:
        return self.name
    
    def GetIndex(self) -> int:
        return self.index

class Graph:

    def __init__(self, nodeNames):
        self.nodes = []
        self.edges = []
        for i, name in enumerate(nodeNames):
            self.nodes.append(Node(i, name))
    
    def AddEdge(self, node_i: int, node_j: int, dist: float):
        self.nodes[node_i].add(self.nodes[node_j])
        self.nodes[node_j].add(self.nodes[node_i])

    def TraverseNodeForCycles(self, node: Node, visited) -> bool:
        i = node.GetIndex()
        if visited[i]:
            return True
        
        visited[i] = True
        children = node.GetChildren()
        
        for child in children:
            if self.TraverseNodeForCycles(child, visited):
                return True
            
        return False

    def ContainsCycles(self) -> bool:
        visited = [False] * len(self.nodes)
        containsCycles = False

        while False in visited:
            unvisited = visited.index(False)
            containsCycles |= self.TraverseNodeForCycles(self.nodes[unvisited], visited)
            if containsCycles:
                break

        return containsCycles

class Christofides:

    def KruskalsMST(map: Map, startIndex: int):
        distances = map.GetDistanceMatrix(startIndex)
        
