﻿using BitSharp.Common;
using BitSharp.Data;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitSharp.Blockchain
{
    public class ChainStateBuilder
    {
        private readonly ChainBuilder chain;
        private readonly UtxoBuilder utxo;
        private readonly BuilderStats stats;

        public ChainStateBuilder(ChainBuilder chain, UtxoBuilder utxo)
        {
            this.chain = chain;
            this.utxo = utxo;
            this.stats = new BuilderStats();
            this.IsConsistent = true;
        }

        ~ChainStateBuilder()
        {
            this.Dispose();
        }

        public bool IsConsistent { get; set; }

        public ChainBuilder Chain { get { return this.chain; } }

        public UtxoBuilder Utxo { get { return this.utxo; } }

        public ChainedBlock LastBlock { get { return this.chain.LastBlock; } }

        public UInt256 LastBlockHash
        {
            get
            {
                var lastBlockLocal = this.LastBlock;
                if (lastBlockLocal != null)
                    return this.LastBlock.BlockHash;
                else
                    return UInt256.Zero;
            }
        }

        public int Height
        {
            get
            {
                var lastBlockLocal = this.LastBlock;
                if (lastBlockLocal != null)
                    return this.LastBlock.Height;
                else
                    return -1;
            }
        }

        public BuilderStats Stats { get { return this.stats; } }

        public void Dispose()
        {
            this.utxo.Dispose();
            GC.SuppressFinalize(this);
        }

        public sealed class BuilderStats
        {
            public long totalTxCount;
            public long totalInputCount;
            public Stopwatch totalStopwatch = new Stopwatch();
            public Stopwatch currentRateStopwatch = new Stopwatch();
            public Stopwatch validateStopwatch = new Stopwatch();
            public long currentBlockCount;
            public long currentTxCount;
            public long currentInputCount;

            internal BuilderStats() { }
        }
    }
}
