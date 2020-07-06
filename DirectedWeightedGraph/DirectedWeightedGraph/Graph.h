#pragma once
using namespace std;
#include <vector>
#include "Vertex.h"

template <typename T>
class Graph
{
public:
	const int& VertexCount = vertexCount;
	Graph();
	void AddVertex(T value);
	void AddEdge(Vertex<T>* startVertex, Vertex<T>* endVertex, int cost);
	bool RemoveVertex(Vertex<T>* vertex);
	bool RemoveEdge(Vertex<T>* startVertex, Vertex<T>* endVertex);
private:
	int vertexCount;
	vector<Vertex<T>> vertices;
};

template <typename T>
Graph<T>::Graph()
{
	vertexCount = 0;
	vertices.clear();
}

template <typename T>
void Graph<T>::AddVertex(T value)
{
	Vertex<T>* newVertex = new Vertex(value);
	vertices.insert(vertices.begin(), newVertex);
	vertexCount++;
}

template  <typename T>
void Graph<T>::AddEdge(Vertex<T>* startVertex, Vertex<T>* endVertex, int cost)
{
	startVertex->AddOutEdge(endVertex, cost);
}

template  <typename T>
bool Graph<T>::RemoveVertex(Vertex<T>* vertex)
{
	vertices.erase(vertex);
	vertexCount--;
}

template  <typename T>
bool Graph<T>::RemoveEdge(Vertex<T>* startVertex, Vertex<T>* endVertex)
{
	startVertex->RemoveOutEdge(endVertex);
}