import argparse
from QoordinateClasses import *
from QQuadraticProblem import QQuadraticProblem
from QOptimizers import *
from QSolutionTranslator import *
from MapReader import ReadMap
from SolutionWriter import WriteSolution

parser = argparse.ArgumentParser()
parser.add_argument("inputPath", help=".map file input", type=str)
parser.add_argument("solver", help="solver type", type=str)
parser.add_argument("outputPath", help=".sol file output", type=str)
args = parser.parse_args()

if __name__ == "__main__":
    inputPath = args.inputPath
    outputPath = args.outputPath
    solver = args.solver

    myMap = ReadMap(inputPath)
    solutionsForTeam = []

    if solver == 'MinimumEigenSolver':
        for i in range(len(myMap.teamList)):
            problem = QQuadraticProblem(myMap.GetDistanceMatrix(i))
            model = problem.GetQuadraticProblemModel('my problem')
            solution = SolveWithNumPyMinimumEigensolver(model)
            solutionsForTeam.append(GetListOfOrder(solution, myMap))
    elif solver == 'BruteForce':
        for i in range(len(myMap.teamList)):
            solutionsForTeam.append(BruteForce(myMap, i))
    
    WriteSolution(myMap, solutionsForTeam, outputPath)

