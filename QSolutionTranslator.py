from qiskit_optimization.algorithms import MinimumEigenOptimizationResult
from QMain import Map

def GetListOfOrder(solution: MinimumEigenOptimizationResult, map: Map):
	buildings = map.GetBuildingNamesList()
	x = solution.x

	N = len(buildings)
	if N * N != len(x):
		raise Exception('Solution and map don\'t match')
	
	order = [None] * N
	
	for b in range(N):
		for t in range(N):
			if x[b * 4 + t] == 1:
				order[t] = buildings[b]
				break
			if(t == N - 1):
				raise Exception(f'Invalid solution: Building {buildings[b]} never visited.')
	
	print('lowest value:', solution.fval)
	print(order)
	return order