from dimod import BinaryQuadraticModel
from QoordinateClasses import *

class DWaveQuadraticProblem:
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
                bin = str(i) + '_' + str(j)
                binaries[i - 1][j - 1] = 'x' + bin
                list.append(bin)
        ''' [[x1_1, x1_2, x1_3], [x2_1, x2_2, x2_3], ...]'''
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

    def __SetConstraintsOf(self, model: BinaryQuadraticModel, binaries):
        for i in range(self.N-1):
            linearRow = []
            linearColumn = []
            for j in range(self.N-1):
                linearRow.append((binaries[i][j], 1))
                linearColumn.append((binaries[j][i], 1))
            model.add_linear_equality_constraint(linearRow, 100, -1)
            model.add_linear_equality_constraint(linearColumn, 100, -1)

            ## exmp: constraint: x11 + x12 + x13 - 1 = 0

        return model

    def CreateBQMfromMap(self, name = '', prettyprint = True) -> BinaryQuadraticModel:
        model = BinaryQuadraticModel(vartype = 'BINARY')
        binaries, list = self.__CreateBinariesMatrix()
        linear = self.__GetLinear(binaries)
        quadratic = self.__GetQuadratic(binaries)

        model.add_linear_from(linear)
        model.add_quadratic_from(quadratic)
        
        #model.binary_var_list(list)
        #model.minimize(constant = 0, linear = linear, quadratic = quadratic)
        self.__SetConstraintsOf(model, binaries)

        if(prettyprint):
            print(model)

        return model