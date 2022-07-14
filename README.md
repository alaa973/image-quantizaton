# image-quantizaton
The idea of color quantization is to reduce the number of colors in an image to a smaller set of representative colors called color palette. Reduction should be  performed so that the quantized image differs as little as possible from the original image
Finding distinct colors:
Description:
We iterate over the image and check if the color of each pixel was seen before if not, we store it in an array called dis_color
Note: we check if the color was seen before through a boolean 3d array of size 256*256*256 to enable us to try all combinations of colors, for example: exist[0][0][255] = 1 means that the color 0,0,225 was seen before and is already stored in the dis_color array 
Minimum spanning tree: 
MST construction steps
First, we calculate the distance between the first distinct color (the first source) and all other colors and we store these edges and mark the first distinct color as visited.
Then, we choose the smallest distance to the destination color
Now, the destination color becomes our new source and we mark it as visited.
We calculate the distance between the source and all other distinct colors except the visited ones and update the edge if we could reach the same destination with less distance than the one already stored in that edge
We repeat the steps from 2 to 4 until the number of edges is equal to the number of distinct colors - 1 (The number of edges in the mst)
Conclusion: we applied prim’s algorithm which builds the MST while calculating the distances as described in the steps above
So we don’t have to use memory to store all the d^2 edges, we only store the edges constructing the MST (d-1 edges) 
The prim’s algorithm enabled us to store the MST edges while calculating the distances, so we don’t have to store all the the graph edges then use them to find the MST, the conditions below enables us to store the MST edges only 
Then we sort the MST edges, and loop on them to calculate the MST cost
Palette generation
 Description:
We iterate over the distinct colors and add each color to the sum color of the cluster to which the color belongs.
Then we divide this sum by the number of colors in the cluster 
Note: we have a struct called node. It has an attribute called parent which represents the cluster id to which a node belongs
Mapping
Description:
We iterate over the image and replace each pixel color with the average of the colors in the cluster to which it belongs
Note: To do the mentioned steps, we need to map each color in the image to an id so that we can know the cluster to which a color belongs.
So we iterate over the image and give each color an id through a 3d array called map
For example: map[0][0][255] = 5 means that the color 0,0,255 has the id 5
Clustering
Description:
The algorithm followed is kruskal disjoint sets with union by rank and path compression
First, we define each node as a separate cluster(each node has itself as a parent and it’s rank is 0)
We iterate over the sorted MST edges, and union the nearest two nodes in one cluster by finding the parent of each node till we reach the desired number of clusters
Find parent: When we call find parent for a node, we traverse up the tree until we reach the root which is the parent of itself. We assign the root as the parent of the node so that we don’t have to traverse the tree each time find parent is called for this node(Path compression)
Union: We join the smaller rank tree to the higher rank tree(if both have equal rank, we join any one of them to the other) 
We stop the algorithm when we reach the desired number of clusters
Finally, we iterate over all the nodes, and find parent of each one of them, so that each node has the root of its cluster as its parent
