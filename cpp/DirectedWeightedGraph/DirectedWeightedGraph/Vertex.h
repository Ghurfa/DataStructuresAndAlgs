#pragma once
using namespace std;
#include <vector>

template <typename T>
class Vertex
{
public:
	const T& Value = value;
	Vertex(T value);
	void AddOutEdge(Vertex<T>* vertex, int cost);
	void RemoveOutEdge(Vertex<T>* vertex);
	bool HasOutEdgeWith(Vertex<T>* vertex);
	int DistanceTo(Vertex<T>* vertex);
private:
	T value;
	vector<pair<Vertex<T>*, int>> outEdges;
};

template <typename T>
Vertex<T>::Vertex(T value)
{
	this->value = value;
}

template <typename T>
void Vertex<T>::AddOutEdge(Vertex<T>* vertex, int cost)
{
	for (auto edge : outEdges)
	{
		if (edge.first == vertex)
		{
			edge.second = cost;
			return;
		}
	}
	outEdges.insert(outEdges.begin(), make_pair(vertex, cost));
}

template <typename T>
void Vertex<T>::RemoveOutEdge(Vertex<T>* vertex)
{
	for (pair<Vertex<T>*, int> edge: outEdges)
	{
		if (edge.first == vertex)
		{
			outEdges.erase(edge);
			return true;
		}
	}
	return false;
}

template <typename T>
bool Vertex<T>::HasOutEdgeWith(Vertex<T>* vertex)
{
	for (pair<Vertex<T>*, int> edge : outEdges)
	{
		if (edge.first == vertex)
		{
			return true;
		}
	}
	return false;
}

template <typename T>
int Vertex<T>::DistanceTo(Vertex<T>* vertex)
{
	for (pair<Vertex<T>*, int> edge : outEdges)
	{
		if (edge.first == vertex)
		{
			return edge.second;
		}
	}
	throw new invalid_argument("Does not have an out edge with that vertex");
}