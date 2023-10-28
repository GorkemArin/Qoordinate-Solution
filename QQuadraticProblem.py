import numpy as np

from qiskit_optimization import QuadraticProgram
from qiskit import BasicAer, Aer
from qiskit.algorithms import QAOA, NumPyMinimumEigensolver
from qiskit_optimization.algorithms import MinimumEigenOptimizer
from qiskit.algorithms.optimizers import COBYLA, SLSQP, ADAM

class QQuadraticProblem:

    def __init__(self, distanceMatrix: np.ndarray):
        if distanceMatrix.ndim != 2:
            raise Exception('Wrong dimensions. 2 is required')

        self.shape = distanceMatrix.shape
        if self.shape[0] != self.shape[1]:
            raise Exception('Invalid shape. Matrix must be square')

        self.distanceMatrix = distanceMatrix
        self.N = self.shape[0]

    def __CreateBinariesMatrix(self):
        binaries = [[''] * (self.N-1) for i in range(self.N-1)]
        list = []
        for i in range(1, self.N):
            for j in range(1, self.N):
                bin = str(i) + str(j)
                binaries[i - 1][j - 1] = 'x' + bin
                list.append(bin)
        ''' [[x11, x12, x13], [x21, x22, x23], ...]'''
        return binaries, list

    def __GetLinear(self, binaries):
        linear = {}
        for i in range(self.N - 1):
            linear.update({binaries[i][0]: self.distanceMatrix[i + 1][0]})

        return linear

    def __GetQuadratic(self, binaries):
        quadratic = {}
        for t in range(self.N-2): #exclude last time
            for cur in range(self.N-1):
                for next in range(self.N-1):
                    if cur == next:
                        continue
                    quadratic.update({(binaries[cur][t], binaries[next][t+1]): self.distanceMatrix[cur][next]})

        return quadratic

    #TODO:
    def __SetConstraintsOf(self, mod: QuadraticProgram, binaries):
        for i in range(self.N-1):
            linearRow = {}
            linearColumn = {}
            for j in range(self.N-1):
                linearRow.update({binaries[i][j] : 1})
                linearColumn.update({binaries[j][i] : 1})
            mod.linear_constraint(linearRow, sense='=', rhs=1)
            mod.linear_constraint(linearColumn, sense='=', rhs=1)
        return mod

    def GetQuadraticProblemModel(self, name = '', prettyprint = True):
        model = QuadraticProgram(name)
        binaries, list = self.__CreateBinariesMatrix()
        linear = self.__GetLinear(binaries)
        quadratic = self.__GetQuadratic(binaries)
        model.binary_var_list(list)
        model.minimize(constant = 0, linear = linear, quadratic = quadratic)
        self.__SetConstraintsOf(model, binaries)

        if(prettyprint):
            print(model.prettyprint())

        return model