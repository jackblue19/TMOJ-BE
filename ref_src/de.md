Trong \*\*học viện thuật toán cổ đại\*\*, có một bài kiểm tra huyền thoại mang tên \*\*Dãy Con Không Giảm Cấp L\*\* — nơi các học giả phải khám phá quy luật ẩn giấu trong những dãy số tưởng chừng vô tận.



Bạn được cho một dãy số nguyên gồm $N$ phần tử $A\_1, A\_2, A\_3, \\dots, A\_N$.  



Một dãy con không giảm cấp L là một dãy các chỉ số $i\_1, i\_2, \\dots, i\_k$ thoả mãn:  



> $A\_{i\_1} \\leq A\_{i\_2} \\leq A\_{i\_3} \\leq \\dots \\leq A\_{i\_k}$  



và khoảng cách giữa các chỉ số liên tiếp phải cách nhau ít nhất L, tức là:  



> $i\_2 - i\_1 \\geq L,$ $i\_3 - i\_2 \\geq L,$ …, $i\_k - i\_{k-1} \\geq L$.  



Nhiệm vụ của bạn là tìm \*\*độ dài lớn nhất\*\* của dãy con không giảm cấp $L$.  



Tuy nhiên, bạn phải trả lời \*\*Q truy vấn\*\* —  mỗi truy vấn cho một giá trị $L$, và bạn cần in ra độ dài lớn nhất tương ứng.



\## \*\*Input\*\*

\- Dòng đầu tiên chứa hai số nguyên $N$ và $Q$ $(1 \\leq N \\leq 5000, 1 \\leq Q \\leq 50000)$ — số phần tử của dãy và số truy vấn.  

\- Dòng thứ hai chứa $N$ số nguyên $A\_1, A\_2, \\dots, A\_N$ $(-10^9 \\leq A\_i \\leq 10^9)$.  

\- Tiếp theo là $Q$ dòng, mỗi dòng chứa một số nguyên $L\_i$ $(1 \\leq L\_i \\leq N)$ — cấp của truy vấn thứ $i$.  





\## \*\*Output\*\*

\- In ra $Q$ dòng, mỗi dòng là \*\*độ dài lớn nhất\*\* của dãy con không giảm cấp tương ứng với truy vấn $L\_i$.



\## \*\*Example\*\*



\### \*\*Sample input 1\*\*

```

6 3

2 1 3 2 4 5

1

2

3

```



\### \*\*Sample output 1\*\*

```

4

3

2

```

\### \*\*Explanation\*\*

