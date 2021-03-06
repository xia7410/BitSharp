﻿using BitSharp.Common;
using BitSharp.Common.ExtensionMethods;
using BitSharp.Core.Domain;
using NLog;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BitSharp.Core.Script
{
    public class PayToPublicKeyHashBuilder
    {
        public byte[] CreateOutputFromPublicKey(byte[] publicKey)
        {
            var sha256 = new SHA256Managed();
            var ripemd160 = new RIPEMD160Managed();
            var publicKeyHash = ripemd160.ComputeHash(sha256.ComputeHash(publicKey));
            return CreateOutputFromPublicKeyHash(publicKeyHash);
        }

        public byte[] CreateOutputFromPublicKeyHash(byte[] publicKeyHash)
        {
            using (var outputScript = new ScriptBuilder())
            {
                outputScript.WriteOp(ScriptOp.OP_DUP);
                outputScript.WriteOp(ScriptOp.OP_HASH160);
                outputScript.WritePushData(publicKeyHash);
                outputScript.WriteOp(ScriptOp.OP_EQUALVERIFY);
                outputScript.WriteOp(ScriptOp.OP_CHECKSIG);

                return outputScript.GetScript();
            }
        }
    }
}
