#pragma once
#include <memory>
using namespace std;

template <typename T>
class DoublyLinkedNode
{
public:
	T Value;
	unique_ptr<DoublyLinkedNode<T>> Next;
	DoublyLinkedNode<T>* Previous;
	DoublyLinkedNode(T value); //, DoublyLinkedNode<T>* next = nullptr, DoublyLinkedNode<T>* previous = nullptr);
};

template <typename T>
DoublyLinkedNode<T>::DoublyLinkedNode(T value) //, DoublyLinkedNode<T>* next, DoublyLinkedNode<T>* previous)
{
	Value = value;
	// Next = make_unique<DoublyLinkedNode<T>>(next);
	// Previous = previous;
}