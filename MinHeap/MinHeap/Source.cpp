#include <iostream>
#include "MinHeap.h"
using namespace std;
int main()
{
	int option = 0;
	MinHeap<int>* minHeap = new MinHeap<int>();

	cout << "Options: " << endl <<
		"0. Insert" << endl <<
		"1. Pop" << endl <<
		"2. Count" << endl <<
		"3. Clear" << endl <<
		"4. Display" << endl <<
		"5. Exit" << endl;
	while (option != 5)
	{
		cout << "Option: ";
		cin >> option;
		switch (option)
		{
		case 0:
		{
			int numToInsert;
			cout << "Number to insert: ";
			cin >> numToInsert;
			minHeap->Insert(numToInsert);
			cout << "Inserted " << numToInsert << endl;
			break;
		}
		case 1:
		{
			int result = minHeap->Pop();
			cout << "Popped " << result << endl;
			break;
		}
		case 2:
			cout << "Count: " << minHeap->Count << endl;
			break;
		case 3:
			minHeap->Clear();
			cout << "Cleared the heap" << endl;
			break;
		case 4:
			minHeap->Display();
			break;
		}
	}
	return 0;
}