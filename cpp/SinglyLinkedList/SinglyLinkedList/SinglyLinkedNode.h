#pragma once
template <typename T>
class SinglyLinkedNode
{
public:
	T Value;
	std::unique_ptr<SinglyLinkedNode<T>> Next;
	SinglyLinkedNode(T value, std::unique_ptr<SinglyLinkedNode<T>> next = {});
};

template<typename T>
SinglyLinkedNode<T>::SinglyLinkedNode(T value, std::unique_ptr<SinglyLinkedNode<T>> next)
{
	Value = value;
	Next = std::move(next);
}
