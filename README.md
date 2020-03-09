# MCSLock

*MCSLock

# Usage

```csharp

System.Threading.MCSLock mCSLock = new System.Threading.MCSLock();

MCSLock.Node node1 = mCSLock.Lock();
// to do .......

mCCLock.Unlock(node1);

```

