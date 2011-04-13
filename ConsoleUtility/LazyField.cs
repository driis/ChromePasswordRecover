using System;

namespace dr.ChromePasswordRecover.ConsoleUtility
{
    /// <summary>
    /// LazyField, takes care of lazy instantation of a field.
    /// (note, this version not thread-safe).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LazyField<T>
    {
        /// <summary>
        /// The instance-
        /// </summary>
        private T instance;

        /// <summary>
        /// Whether we have yet instantiated the instance.
        /// </summary>
        private bool instantiated = false;
        /// <summary>
        /// The delegate, that will 
        /// </summary>
        private readonly Func<T> instantiator;

        /// <summary>
        /// Initializes a new instance of the <see cref="LazyField&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="instantiator">The instantiator.</param>
        public LazyField(Func<T> instantiator)
        {
            if (instantiator == null)
                throw new ArgumentNullException("instantiator");
            this.instantiator = instantiator;
        }


        /// <summary>
        /// Gets the value of this lazy field. If this is the first access, the instance will be created.
        /// </summary>
        /// <value>The value.</value>
        public T Value
        {
            get
            {
                if (!instantiated)
                {
                    instance = instantiator();
                    instantiated = true;
                }
                return instance;
            }
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="dr.Commons.LazyField&lt;T&gt;"/> to <see cref="T"/>.
        /// </summary>
        /// <param name="rhs">The RHS.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator T(LazyField<T> rhs)
        {
            return rhs == null ? default(T) : rhs.Value;
        }        
    }

    /// <summary>
    /// Lazy field accessors (extension methods and the option to use generic type inference).
    /// </summary>
    public static class LazyField
    {
        /// <summary>
        /// Gets the specified instantiator.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instantiator">The instantiator.</param>
        /// <returns></returns>
        public static LazyField<T> Get<T>(Func<T> instantiator)
        {
            return new LazyField<T>(instantiator);
        }

        /// <summary>
        /// Ases the lazy.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instantiator">The instantiator.</param>
        /// <returns></returns>
        public static LazyField<T> AsLazy<T>(this Func<T> instantiator)
        {
            return Get(instantiator);
        }
    }
}
