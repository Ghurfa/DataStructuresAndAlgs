#include <iostream>
using namespace std;
int* mergeArrays(int* leftArr, int leftLength, int* rightArr, int rightLength);
int* mergeSort(int* arr, int length)
{
	if (length <= 1) return arr;
	int leftLength = (int)(length / 2);
	int rightLength = length - leftLength;
	int* leftArr = arr;
	int* rightArr = &arr[leftLength];
	leftArr = mergeSort(leftArr, leftLength);
	rightArr = mergeSort(rightArr, rightLength);
	return mergeArrays(mergeSort(leftArr, leftLength), leftLength, rightArr, rightLength);
}
int* mergeArrays(int* leftArr, int leftLength, int* rightArr, int rightLength)
{
	int newArrLength = leftLength + rightLength;
	int* newArr = new int[newArrLength];
	int* leftPtr = leftArr;
	int* rightPtr = rightArr;
	for (int i = 0; i < newArrLength; i++)
	{
		if (rightPtr >= rightArr + rightLength)
		{
			newArr[i] = *leftPtr;
			leftPtr++;
		}
		else if(leftPtr >= leftArr + leftLength || *rightPtr < *leftPtr)
		{
			newArr[i] = *rightPtr;
			rightPtr++;
		}
		else
		{
			newArr[i] = *leftPtr;
			leftPtr++;
		}
	}
	return newArr;
}

int main()
{
	cout << "Length: " << endl;
	int length = 0;
	cin >> length;
	cout << "Values: " << endl;
	int* arr = new int[length];
	for (int i = 0; i < length; i++)
	{
		cin >> arr[i];
	}

	arr = mergeSort(arr, length);
	cout << length << endl;
	for (int i = 0; i < length; i++)
	{
		cout << arr[i] << ", ";
	}
	cout << endl;
	while (true);
	return 0;
}