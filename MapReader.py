from QoordinateClasses import *
from enum import Enum
from typing import List

class Context(Enum):
    none = 0
    map = 1
    buildings = 2
    teams = 3

context = Context.none
buildings = []
teams = []
map = None
mapname = ''

def InitVariables():
    context = Context.none
    buildings = []
    teams = []
    map = None
    mapname = ''

def SplitData(line: str):
    dataList = line.split('|')
    dataList = [s.strip() for s in dataList]
    return dataList

def ReadMapData(dataList: List[str]):
    global mapname
    name = ''

    for data in dataList:
        if data.startswith('mapname:'):
            name = data.split(':', 1)[1].strip()
            if name[0] == '"' and name[-1] == '"' :
                name = name[1:-1]
            mapname = name

def ReadBuilding(dataList: List[str]):
    name = ''
    point = Point(0, 0)
    need = -1

    for data in dataList:
        if data.startswith('name:'):
            name = data.split(':', 1)[1].strip()
            if name[0] == '"' and name[-1] == '"' :
                name = name[1:-1]
        elif data.startswith('point:'):
            pointStr = data.split(':', 1)[1].strip()
            pointList = pointStr.split(',')
            x = int(pointList[0].strip())
            y = int(pointList[1].strip())
            point = Point(x, y)
        elif data.startswith('need:'):
            needStr = data.split(':', 1)[1].strip()
            need = int(needStr.strip())
    
    buildings.append(Building(point, need, name))


def ReadTeam(dataList: List[str]):
    name = ''
    point = Point(0, 0)
    cap = -1

    for data in dataList:
        if data.startswith('name:'):
            name = data.split(':', 1)[1].strip()
            if name[0] == '"' and name[-1] == '"' :
                name = name[1:-1]
        elif data.startswith('point:'):
            pointStr = data.split(':', 1)[1].strip()
            pointList = pointStr.split(',')
            x = int(pointList[0].strip())
            y = int(pointList[1].strip())
            point = Point(x, y)
        elif data.startswith('need:'):
            capStr = data.split(':', 1)[1].strip()
            cap = int(capStr.strip())
    
    teams.append(Team(point, cap, name))

def ReadLine(line: str):
    global context

    if len(line) == 0:
        return
    
    if line.startswith('>'):
        line = line[1:].strip()
        
        if line.startswith('map'):
            context = Context.map
        elif line.startswith('buildings'):
            context = Context.buildings
        elif line.startswith('teams'):
            context = Context.teams
        else:
            context = Context.none
        return
    
    if line.startswith('*'):
        line = line[1:].strip()
        dataList = SplitData(line)
        if context == Context.map:
            ReadMapData(dataList)
        elif context == Context.buildings:
            ReadBuilding(dataList)
        elif context == Context.teams:
            ReadTeam(dataList)
        else:
            return

def ReadMap(filename: str) -> Map:
    filepath = 'Maps\\' + filename
    InitVariables()

    with open(filepath, 'r') as file:
        
        for line in file:
            line = line.strip()
            ReadLine(line)
    
    return Map(teams, buildings, mapname)