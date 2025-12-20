//@THANDUCBAO
//codeforces: ducbao_
//atcoder: ThanDucBao
//topcoder: ducbao_
#include <bits/stdc++.h>
#include <random>
#include <time.h>
#define int long long
#define endl '\n'
#define BIG __int128
#define mod 1000000007
#define INF 1e18
template<class A, class B> bool maximize(A &x, const B &y) { if (x < y) { x = y; return true; } return false; }
template<class A, class B> bool minimize(A &x, const B &y) { if (x > y) { x = y; return true; } return false; }
using namespace std;
mt19937 rng(chrono::steady_clock::now().time_since_epoch().count());
int R(int l,int r){
    return uniform_int_distribution<int>(l,r)(rng);
}
void sinh(){
    int n=R(1,1000000000);
    cout<<n<<endl;
}
int power(int a,int b){
    if(b==0)return 1;
    if(b==1)return a;
    int z=power(a,b/2);
    if(b%2==0)return z*z;
    return z*z*a;
}
int n,q;
vector<int>a;
int ans(int x) {
    int ans=0;
    for (int z=1;z<=x;z++) {
        vector<int>t;
        for (int i=z;i<=n;i+=x) {
            int p=a[i];
            auto it=upper_bound(t.begin(),t.end(),p);
            if(it==t.end())t.push_back(p);
            else *it=p;
        }
        ans=max(ans,(int)t.size());
    }
    return ans;
}
void solve(){
    cin>>n>>q;
    a.resize(n+1);
    for(int i=1;i<=n;i++){
        cin>>a[i];
    }
    while(q--){
        int x;
        cin>>x;
        cout<<ans(x)<<endl;
    }
}
signed main(){
    ios_base::sync_with_stdio(false);
    cin.tie(nullptr);cout.tie(nullptr);
    #ifndef ONLINE_JUDGE
    freopen("input.txt","r",stdin);
    freopen("output.txt","w",stdout);
    #endif
    int numTest=1;
    //cin>>numTest;
    while(numTest--){
        solve();
    }
    return 0;
}
//author: ducbao_
// from tay son secondary school
