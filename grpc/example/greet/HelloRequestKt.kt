//Generated by the protocol buffer compiler. DO NOT EDIT!
// source: greet.proto

package greet;

@kotlin.jvm.JvmSynthetic
public inline fun helloRequest(block: greet.HelloRequestKt.Dsl.() -> kotlin.Unit): greet.Greet.HelloRequest =
  greet.HelloRequestKt.Dsl._create(greet.Greet.HelloRequest.newBuilder()).apply { block() }._build()
public object HelloRequestKt {
  @kotlin.OptIn(com.google.protobuf.kotlin.OnlyForUseByGeneratedProtoCode::class)
  @com.google.protobuf.kotlin.ProtoDslMarker
  public class Dsl private constructor(
    private val _builder: greet.Greet.HelloRequest.Builder
  ) {
    public companion object {
      @kotlin.jvm.JvmSynthetic
      @kotlin.PublishedApi
      internal fun _create(builder: greet.Greet.HelloRequest.Builder): Dsl = Dsl(builder)
    }

    @kotlin.jvm.JvmSynthetic
    @kotlin.PublishedApi
    internal fun _build(): greet.Greet.HelloRequest = _builder.build()

    /**
     * <code>string name = 1;</code>
     */
    public var name: kotlin.String
      @JvmName("getName")
      get() = _builder.getName()
      @JvmName("setName")
      set(value) {
        _builder.setName(value)
      }
    /**
     * <code>string name = 1;</code>
     */
    public fun clearName() {
      _builder.clearName()
    }
  }
}
@kotlin.jvm.JvmSynthetic
public inline fun greet.Greet.HelloRequest.copy(block: greet.HelloRequestKt.Dsl.() -> kotlin.Unit): greet.Greet.HelloRequest =
  greet.HelloRequestKt.Dsl._create(this.toBuilder()).apply { block() }._build()
