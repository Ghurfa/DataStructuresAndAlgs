#include <iostream>
#include "DoublyLinkedList.h"
using namespace std;
int main()
{
	int option = 0;
	DoublyLinkedList<int>* list = new DoublyLinkedList<int>;

	cout << "Options: " << endl <<
		"0. AddFirst" << endl <<
		"1. AddLast" << endl <<
		"2. RemoveFirst" << endl <<
		"3. RemoveLast" << endl <<
		"4. Contains" << endl <<
		"5. Count" << endl <<
		"6. Clear" << endl <<
		"7. Exit" << endl;
	while (option != 7)
	{
		cin >> option;
		switch (option)
		{
		case 0:
		{
			int numToAdd;
			cout << "Number to add: ";
			cin >> numToAdd;
			list->AddFirst(numToAdd);
			cout << "Added " << numToAdd << " at the front" << endl;
			break;
		}
		case 1:
		{
			int numToAdd;
			cout << "Number to add: ";
			cin >> numToAdd;
			list->AddLast(numToAdd);
			cout << "Added " << numToAdd << " at the end" << endl;
			break;
		}
		case 2:
		{
			bool result = list->RemoveFirst();
			if (result)
			{
				cout << "Removed first value" << endl;
			}
			else
			{
				cout << "Failed to remove first value" << endl;
			}
			break;
		}
		case 3:
		{
			bool result = list->RemoveLast();
			if (result)
			{
				cout << "Removed last value" << endl;
			}
			else
			{
				cout << "Failed to remove last value" << endl;
			}
			break;
		}
		case 4:
		{
			int numberToSearchFor = 0;
			cout << "Number to search for: ";
			cin >> numberToSearchFor;
			bool containsNum = list->Contains(numberToSearchFor);
			if (containsNum)
			{
				cout << "List contains " << numberToSearchFor << endl;
			}
			else
			{
				cout << "List does not contains " << numberToSearchFor << endl;
			}
			break;
		}
		case 5:
			cout << "Count: " << list->Count << endl;
			break;

		case 6:
			list->Clear();
			cout << "Cleared list" << endl;
			break;
		}
	}
	return 0;
}