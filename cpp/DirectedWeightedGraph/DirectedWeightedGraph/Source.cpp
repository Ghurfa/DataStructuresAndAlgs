#include <iostream>

#include "Vertex.h"


using namespace std;


int main()
{
	Vertex<int>* testVertex1 = new Vertex<int>(1);
	Vertex<int>* testVertex2 = new Vertex<int>(2);
	testVertex1->AddOutEdge(testVertex2, 2);
	return 0;

}