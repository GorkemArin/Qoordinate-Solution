from collections import namedtuple
from math import sqrt
from copy import deepcopy
import numpy as np
from typing import List

Point = namedtuple('Point', 'x y')

class Building:
    def __init__(self, coordinate: Point, need: int, name: str = ""):
        self.coordinate = coordinate
        self.need = need
        self.name = name

class Team:
    def __init__(self, coordinate: Point, capacity: int, name: str = ""):
        self.coordinate = coordinate
        self.capacity = capacity
        self.name = name

class Map:
    buildingsDistanceMatrix = None

    def __GetDistance(self, point1: Point, point2: Point) -> float:
        return sqrt((point1.x - point2.x) ** 2 + (point1.y - point2.y) ** 2)

    def __GetBuildingsDistanceMatrix(self):
        N: int = len(self.buildingList) + 1
        matrix = np.zeros((N, N))

        for i in range(1, N):
            for j in range(i + 1, N):
                firstPoint = self.buildingList[i - 1].coordinate
                secondPoint = self.buildingList[j - 1].coordinate
                distance = self.__GetDistance(firstPoint, secondPoint)
                matrix[i, j] = matrix[j, i] = distance

        return matrix

    def __init__(self, teamList: List[Team], buildingList: List[Team], mapname: str = ''):
        self.teamList = teamList
        self.buildingList = buildingList
        self.buildingsDistanceMatrix = self.__GetBuildingsDistanceMatrix()

    #Index 0 is team
    def GetDistanceMatrix(self, teamIndex: int):
        pointTeam = self.teamList[teamIndex].coordinate
        N: int = len(self.buildingList)
        matrix = deepcopy(self.buildingsDistanceMatrix)

        for i in range(N):
            distance = self.__GetDistance(pointTeam, self.buildingList[i].coordinate)
            matrix[0, i + 1] = matrix[i + 1, 0] = distance

        return matrix

    def GetBuildingNamesList(self):
        names = [b.name for b in self.buildingList]
        '''for b in self.buildingList:
            names.append(b.name)'''
        return names