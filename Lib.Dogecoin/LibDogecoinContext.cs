namespace Lib.Dogecoin
{
	/// <summary>
	/// There is a non-zero chance that this is supposed to be freeing up resources
	/// that is is currently not. Need to test.
	/// </summary>
	public class LibDogecoinContext : IDisposable
	{
		private static object _lock = new object();
		private static LibDogecoinContext _instance;

		private bool _disposed = false;


		public static LibDogecoinContext CreateContext()
		{
			lock (_lock)
			{
				if(_instance == null || _instance._disposed)
				{
					_instance = new LibDogecoinContext();
					return _instance;
				}

				throw new Exception("Already using a LibDogecoinContext.");
			}
		}


		private LibDogecoinContext()
		{
			LibDogecoinInterop.dogecoin_ecc_start();
		}


		public (string privateKey, string publicKey) GeneratePrivPubKeyPair(bool testNet = false)
		{
			lock(_lock)
			{
				var privKey = new char[52];
				var pubKey = new char[34];

				LibDogecoinInterop.generatePrivPubKeypair(privKey, pubKey, testNet);

				return (new string(privKey), new string(pubKey));
			}
		}

		public (string privateKey, string publicKey) GenerateHDMasterPubKeyPair(bool testNet = false)
		{
			lock (_lock)
			{
				var privKey = new char[111];
				var pubKey = new char[34];

				LibDogecoinInterop.generateHDMasterPubKeypair(privKey, pubKey, testNet);

				return (new string(privKey), new string(pubKey));
			}
		}


		public string GenerateDerivedHDPubKey(string masterPrivKey)
		{
			lock (_lock)
			{
				var pubKey = new char[34];

				LibDogecoinInterop.generateDerivedHDPubkey(masterPrivKey.ToCharArray(), pubKey);

				return new string(pubKey);
			}
		}

		public string GetDerivedHDAddressByPath(string masterPrivKey, string derivedPath, bool isPriv)
		{
			lock (_lock)
			{
				var key = new char[111];

				LibDogecoinInterop.getDerivedHDAddressByPath(
					masterPrivKey.ToCharArray(),
					derivedPath.ToCharArray(),
					key,
					isPriv);

				return new string(key);
			}
		}

		public bool VerifyPrivPubKeyPair(string privKey, string pubKey, bool testNet = false)
		{
			lock (_lock)
			{
				return 0 != LibDogecoinInterop.verifyPrivPubKeypair(privKey.ToCharArray(), pubKey.ToCharArray(), testNet);
			}
		}

		public bool VerifyHDMasterPubKeyPair(string privKey, string pubKey, bool testNet = false)
		{
			lock (_lock)
			{
				return 0 != LibDogecoinInterop.verifyHDMasterPubKeypair(privKey.ToCharArray(), pubKey.ToCharArray(), testNet);
			}
		}

		public bool VerifyP2pkhAddress(string pubKey)
		{
			lock (_lock)
			{
				return 0 != LibDogecoinInterop.verifyP2pkhAddress(pubKey.ToCharArray(), (uint)pubKey.Length);
			}
		}


		public string GenerateEnglishMnemonic(string entropy, string entropySize)
		{
			lock (_lock)
			{
				var mnemonic = new char[1024];

				LibDogecoinInterop.generateEnglishMnemonic(entropy.ToCharArray(), entropySize.ToCharArray(), mnemonic);

				return new string(mnemonic);
			}
		}


		public string GenerateRandomEnglishMnemonic(string entropySize)
		{
			lock (_lock)
			{
				var mnemonic = new char[1024];

				LibDogecoinInterop.generateRandomEnglishMnemonic(entropySize.ToCharArray(), mnemonic);

				return new string(mnemonic).TrimEnd('\0');
			}
		}
		
		public string GetDerivedHDAddressFromMnemonic(int account, int index, string changeLevel, string mnemonic, string password, bool testNet)
		{
			lock (_lock)
			{
				var address = new char[1024];

				LibDogecoinInterop.getDerivedHDAddressFromMnemonic(
					(uint)account,
					(uint)index,
					changeLevel.ToCharArray(),
					mnemonic.ToCharArray(),
					password.ToCharArray(),
					address,
					testNet);

				return new string(address).TrimEnd('\0');
			}
		}


		public string P2pkhToQrString(string p2pkh)
		{
			lock(_lock)
			{
				var qrString = new char[3918 * 4];

				LibDogecoinInterop.qrgen_p2pkh_to_qr_string(p2pkh.ToCharArray(), qrString);

				return new string(qrString).TrimEnd('\0');
			}
		}


		public int StartTransaction()
		{
			lock (_lock)
			{
				return LibDogecoinInterop.start_transaction();
			}
		}


		public bool AddUTXO(int txIndex, string hexUTXOTxId, int vOut)
		{
			lock (_lock)
			{
				return 0 != LibDogecoinInterop.add_utxo(txIndex, hexUTXOTxId.ToCharArray(), vOut);
			}
		}


		public bool AddOutput(int txIndex, string destinationAddress, string amount)
		{
			lock(_lock)
			{
				return 0 != LibDogecoinInterop.add_output(txIndex, destinationAddress.ToCharArray(), amount.ToCharArray());
			}
		}


		public string FinalizeTransaction(int txIndex, string destinationAddress, string fee, string outputSum, string changeAddress)
		{
			lock (_lock)
			{
				return new string(LibDogecoinInterop.finalize_transaction(
										txIndex,
										destinationAddress.ToCharArray(),
										fee.ToCharArray(),
										outputSum.ToCharArray(),
										changeAddress.ToCharArray()));
			}
		}

		public string GetRawTransaction(int txIndex)
		{
			lock (_lock)
			{
				return new string(LibDogecoinInterop.get_raw_transaction(txIndex));
			}
		}

		public string SignMessage(string privateKey, string message)
		{
			lock (_lock)
			{
				return new string(LibDogecoinInterop.sign_message(privateKey.ToCharArray(), message.ToCharArray()));
			}
		}

		public bool VerifyMessage(string signature, string message, string address)
		{
			lock (_lock)
			{
				return LibDogecoinInterop.verify_message(signature.ToCharArray(), message.ToCharArray(), address.ToCharArray());
			}
		}


		public void Dispose()
		{
			lock(_lock)
			{
				_disposed = true;
				LibDogecoinInterop.dogecoin_ecc_stop();
			}

		}


		public const string ENTROPY_SIZE_128 = "128";
		public const string ENTROPY_SIZE_256 = "256";
	}
}