import argparse
from QoordinateClasses import *
#from QQuadraticProblem import QQuadraticProblem
#from QOptimizers import *
#from QSolutionTranslator import *
#from SolutionWriter import WriteSolution
import xml.etree.ElementTree as ET
from xml.etree.ElementTree import Element, SubElement, tostring
from xml.dom import minidom
import itertools

parser = argparse.ArgumentParser()
parser.add_argument("inputPath", help=".map file input", type=str)

parser.add_argument("outputPath", help=".sol file output", type=str)
args = parser.parse_args()

def ReadXMLMap(mapPath: str):
    tree = ET.parse(mapPath)
    root = tree.getroot()
    
    map_attrib = root.attrib
    map_name = map_attrib['name']

    buildings = []
    teams = []

    for child in root:
        if child.tag == 'building_list':
            for build in child:
                name = build.attrib['name']
                x = 0.0
                y = 0.0

                for param in build:
                    if(param.tag == 'x'):
                        x = float(param.text)
                    elif(param.tag == 'y'):
                        y = float(param.text)
                buildings.append(Building(Point(x,y), 5, name))

        elif child.tag == 'team_list':
            for team in child:
                name = team.attrib['name']
                x = 0.0
                y = 0.0

                for param in team:
                    if(param.tag == 'x'):
                        x = float(param.text)
                    elif(param.tag == 'y'):
                        y = float(param.text)
                teams.append(Team(Point(x,y), 5, name))
            
    return Map(teams, buildings, map_name)

def WriteXMLSolution(map: Map, solution, outputPath: str):
    teamNames = map.GetTeamNamesList()

    top = Element('solution')
    top.set('name', map.GetName())
    for i, name in enumerate(teamNames):
        if i >= len(solution):
            continue

        child = SubElement(top, 'team')
        child.set('name', name)
        order = SubElement(child, 'building_order')

        sol = solution[i]
        orderTxt = '['
        for buildname in sol:
            orderTxt += buildname + ','
        orderTxt = orderTxt[:-1] + ']'

        order.text = orderTxt

    xmlstr = minidom.parseString(ET.tostring(top)).toprettyxml(indent="   ")
    with open(outputPath, "w") as f:
        f.write(xmlstr)

    # with open(outputPath, 'w') as f:
    #     f.write(tostring(top).decode('utf-8'))

def BruteForce(map: Map, teamIndex: int = 0):
    matrix = map.GetDistanceMatrix(teamIndex)
    N = matrix.shape[0]
    perm = list(itertools.permutations(range(1, N)))
    
    alldistances = []
    mindistance = float("inf")
    minorder = None

    for order in perm:
        dist = 0
        for i in range(len(order)):
            if(i == 0):
                dist = dist + matrix[0, order[i]]
                continue
            dist = dist + matrix[order[i], order[i-1]]
        
        if dist < mindistance:
            mindistance = dist
            minorder = order

        alldistances.append(dist)

    print('all distances:', alldistances)
    print('min distance:', mindistance)
    print('min order:', minorder)
    
    names = map.GetBuildingNamesList()
    name_order = [names[i - 1] for i in minorder]

    print('min name order:', name_order)
    return name_order

if __name__ == "__main__":
    inputPath = args.inputPath
    outputPath = args.outputPath
    solver = 'BruteForce'

    myMap = ReadXMLMap(inputPath)
    solutionsForTeam = []

    #if solver == 'MinimumEigenSolver':
        # for i in range(len(myMap.teamList)):
        #     problem = QQuadraticProblem(myMap.GetDistanceMatrix(i))
        #     model = problem.GetQuadraticProblemModel('my problem')
        #     solution = SolveWithNumPyMinimumEigensolver(model)
        #     solutionsForTeam.append(GetListOfOrder(solution, myMap))
    if solver == 'BruteForce':
        for i in range(len(myMap.teamList)):
            solutionsForTeam.append(BruteForce(myMap, i))
    
    WriteXMLSolution(myMap, solutionsForTeam, outputPath)
    
    
    #WriteSolution(myMap, solutionsForTeam, outputPath)

