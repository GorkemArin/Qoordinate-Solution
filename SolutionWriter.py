from QoordinateClasses import *

def WriteSolution(map: Map, solution, outputPath: str):
    teamNames = map.GetTeamNamesList()
    with open(outputPath, 'w') as f:
        for i, name in enumerate(teamNames):
            if i >= len(solution):
                continue

            f.write(f'> {name}:')
            sol = solution[i]
            
            line = ' '
            for build in sol:
                line += f'"{build}", '

            line = line[:-2] + '\n'  
            f.write(line)
    